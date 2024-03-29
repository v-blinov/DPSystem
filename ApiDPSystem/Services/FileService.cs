﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Transactions;
using ApiDPSystem.FileFormat.Json.UniversalReadVersion;
using ApiDPSystem.Models;
using ApiDPSystem.Models.Parser;
using ApiDPSystem.Repository.Interfaces;
using ApiDPSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Text;
using YamlDotNet.Serialization;
using Car = ApiDPSystem.Entities.Car;
using JsonSerializer = System.Text.Json.JsonSerializer;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace ApiDPSystem.Services
{
    public class FileService
    {
        private readonly ICarRepository _carRepository;
        private readonly IDataCheckerService _dataChecker;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };

        public FileService(IDataCheckerService dataCheckerService, ICarRepository carRepository)
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

        public ActionResult CreateFile(string fileName, Filter filter)
        {
            var carEntities = _carRepository.GetFullCarsInfoWithFilter(filter);

            var convertToStringMethod = ChooseConvertMethodByFormat(filter);
            var carsInfoInStringAsJson = convertToStringMethod(carEntities);

            var byteArray = Encoding.UTF8.GetBytes(carsInfoInStringAsJson);
            var fileContentResult = new FileContentResult(byteArray, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
            return fileContentResult;
        }

        private Func<IEnumerable<Car>, string> ChooseConvertMethodByFormat(Filter filter) =>
            filter.FileFormat switch
            {
                Models.FileFormat.json => ConvertToJsonString,
                Models.FileFormat.xml => ConvertToXmlString,
                Models.FileFormat.yaml => ConvertToYamlString,
                Models.FileFormat.csv => ConvertToCsvString,
                _ => ConvertToJsonString
            };

        private string ConvertToJsonString(IEnumerable<Car> carEntities)
        {
            var jsonUniversalVersion = carEntities.Select(FileFormat.Json.UniversalReadVersion.Car.ConvertFromDbModel).ToList();
            var root = new Root
            {
                Cars = jsonUniversalVersion
            };

            return JsonSerializer.Serialize(root, _jsonSerializerOptions);
        }
        private string ConvertToXmlString(IEnumerable<Car> carEntities)
        {
            var xmlUniversalVersion = carEntities.Select(FileFormat.Xml.UniversalReadVersion.Car.ConvertFromDbModel).ToList();
            var root = new FileFormat.Xml.UniversalReadVersion.Root
            {
                Cars = xmlUniversalVersion
            };

            using var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof(FileFormat.Xml.UniversalReadVersion.Root));
            serializer.Serialize(writer, root);
            return writer.ToString();
        }
        private string ConvertToYamlString(IEnumerable<Car> carEntities)
        {
            var yamlUniversalVersion = carEntities.Select(FileFormat.Yaml.UniversalReadVersion.Car.ConvertFromDbModel).ToList();
            var root = new FileFormat.Yaml.UniversalReadVersion.Root
            {
                Cars = yamlUniversalVersion
            };

            using var writer = new StringWriter();
            var serializer = new SerializerBuilder().Build();
            serializer.Serialize(writer, root);

            return writer.ToString();
        }
        private string ConvertToCsvString(IEnumerable<Car> carEntities)
        {
            var csvUniversalVersion = carEntities.Select(FileFormat.Csv.UniversalReadVersion.Car.ConvertFromDbModel).ToList();
            var root = new FileFormat.Csv.UniversalReadVersion.Root
            {
                Cars = csvUniversalVersion
            };

            var str = CsvSerializer.SerializeToString(root);

            return str;
        }
    }
}