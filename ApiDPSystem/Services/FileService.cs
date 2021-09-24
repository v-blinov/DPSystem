using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Transactions;
using ApiDPSystem.Models.Parser;
using ApiDPSystem.Repository;
using ApiDPSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Serialization;
using ApiDPSystem.Interfaces;
using ApiDPSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiDPSystem.Services
{
    public class FileService
    {
        private readonly IDataCheckerService _dataChecker;
        private readonly CarRepository _carRepository;
        
        private readonly JsonSerializerOptions _jsonSerializerOptions = new ()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };

        public FileService(IDataCheckerService dataCheckerService, CarRepository carRepository)
        {
            _dataChecker = dataCheckerService;
            _carRepository = carRepository;
        }

        public async Task ProcessFileAsync(IFormFile file, string dealer)
        {
            var fileReadTask = ReadFileAsync(file);
            var fileExtension = Path.GetExtension(file.FileName);

            var parser = Selector.GetParser(fileExtension);

            var fileContent = await fileReadTask;
            var dbModels = parser.Parse(fileContent, file.FileName, dealer);

            using (var transaction = new TransactionScope())
            {
                _dataChecker.MarkSoldCars(dbModels, dealer);
                _dataChecker.SetToDatabase(dbModels);

                transaction.Complete();
            }
        }

        private static async Task<string> ReadFileAsync(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            return await reader.ReadToEndAsync();
        }

        public ActionResult CreateJsonFile(string fileName, Filter filter)
        {
            var carEntities = _carRepository.GetFullCarsInfoWithFilter(filter);
            var carsInfoInStringAsJson = ConvertToJsonString(carEntities);
            var byteArray = Encoding.UTF8.GetBytes(carsInfoInStringAsJson);
            var fileContentResult = new FileContentResult(byteArray, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
            return fileContentResult;
        }
        
        private string ConvertToJsonString(IEnumerable<Entities.Car> carEntities)
        {
            var jsonUniversalVersion = carEntities.Select(FileFormat.Json.UniversalReadVersion.Car.ConvertFromDbModel).ToList();
            var root = new FileFormat.Json.UniversalReadVersion.Root()
            {
                Cars = jsonUniversalVersion
            };

            return  System.Text.Json.JsonSerializer.Serialize(root, _jsonSerializerOptions);
        }
        
        
        public ActionResult CreateXmlFile(string fileName, Filter filter)
        {
            var carEntities = _carRepository.GetFullCarsInfoWithFilter(filter);
            var carsInfoInStringAsJson = ConvertToXmlString(carEntities);
            
            var byteArray = Encoding.UTF8.GetBytes(carsInfoInStringAsJson);
            var fileContentResult = new FileContentResult(byteArray, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
            return fileContentResult;
        }

        private string ConvertToXmlString(IEnumerable<Entities.Car> carEntities)
        {
             var xmlUniversalVersion = carEntities.Select(FileFormat.Xml.UniversalReadVersion.Car.ConvertFromDbModel).ToList();
             var root = new FileFormat.Xml.UniversalReadVersion.Root()
             {
                 Cars = xmlUniversalVersion
             };
             
             using var writer = new StringWriter();
             var serializer = new XmlSerializer(typeof(FileFormat.Xml.UniversalReadVersion.Root));
             serializer.Serialize(writer, root);
             return writer.ToString();
        }
        
        
        public ActionResult CreateYamlFile(string fileName, Filter filter)
        {
            var carEntities = _carRepository.GetFullCarsInfoWithFilter(filter);
            var carsInfoInStringAsJson = ConvertToYamlString(carEntities);
            
            var byteArray = Encoding.UTF8.GetBytes(carsInfoInStringAsJson);
            var fileContentResult = new FileContentResult(byteArray, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
            return fileContentResult;
        }
        
        
        private string ConvertToYamlString(IEnumerable<Entities.Car> carEntities)
        {
            var yamlUniversalVersion = carEntities.Select(FileFormat.Yaml.UniversalReadVersion.Car.ConvertFromDbModel).ToList();
            var root = new FileFormat.Yaml.UniversalReadVersion.Root()
            {
                Cars = yamlUniversalVersion
            };

            using var writer = new StringWriter();
            var serializer = new SerializerBuilder().Build();
            serializer.Serialize(writer, root);
            
            return writer.ToString();
        }
    }
}