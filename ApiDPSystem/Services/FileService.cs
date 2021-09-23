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
        
        
        
        public string GetActualCarsInStringAsJson(string dealerName)
        {
            Func<string, List<Entities.Car>> repositoryCarChoosingMethod = _carRepository.GetFullActualCarsInfoForDealer;
            return ConvertToJsonString(repositoryCarChoosingMethod, dealerName);
        }
        
        public string GetSoldCarsInStringAsJson(string dealerName)
        {
            Func<string, List<Entities.Car>> repositoryCarChoosingMethod = _carRepository.GetFullSoldCarsInfoForDealer;
            return ConvertToJsonString(repositoryCarChoosingMethod, dealerName);
        }
        
        public string GetAllHistoryInStringAsJson(string dealerName)
        {
            Func<string, List<Entities.Car>> repositoryCarChoosingMethod = _carRepository.GetFullHistoryInfoForDealer;
            return ConvertToJsonString(repositoryCarChoosingMethod, dealerName);
        }
        
        
        
        public ActionResult CreateJsonFile(Func<string, string> getCarsMethod, string fileName, string dealerName)
        {
            var fileContentInString = getCarsMethod(dealerName);
            var byteArray = Encoding.UTF8.GetBytes(fileContentInString);
            var fileContentResult = new FileContentResult(byteArray, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
            return fileContentResult;
        }
        
        private string ConvertToJsonString(Func<string, List<Entities.Car>> repositoryCarChoosingMethod, string dealerName)
        {
            var actualCars = repositoryCarChoosingMethod(dealerName);
            
            var jsonUniversalVersion = actualCars.Select(FileFormat.Json.UniversalReadVersion.Car.ConvertFromDbModel).ToList();

            return JsonSerializer.Serialize(jsonUniversalVersion, _jsonSerializerOptions);
        }
    }
}