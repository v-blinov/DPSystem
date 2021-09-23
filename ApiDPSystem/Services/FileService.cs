using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Transactions;
using ApiDPSystem.Models.Parser;
using ApiDPSystem.Repository;
using ApiDPSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Unicode;

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
            var jsonV1Cars = new List<FileFormat.Json.Version1.Car>();

            var actualCars = _carRepository.GetFullActualCarsInfoForDealer(dealerName);
            foreach (var dbModelCar in actualCars)
                jsonV1Cars.Add(new FileFormat.Json.Version1.Car().ConvertFromDbModel(dbModelCar) as FileFormat.Json.Version1.Car);
            
            return JsonSerializer.Serialize(jsonV1Cars, _jsonSerializerOptions);
        }
        
        public string GetSoldCarsInStringAsJson(string dealerName)
        {
            var jsonV1Cars = new List<FileFormat.Json.Version1.Car>();

            var actualCars = _carRepository.GetFullSoldCarsInfoForDealer(dealerName);
            foreach (var dbModelCar in actualCars)
                jsonV1Cars.Add(new FileFormat.Json.Version1.Car().ConvertFromDbModel(dbModelCar) as FileFormat.Json.Version1.Car);
            
            return JsonSerializer.Serialize(jsonV1Cars, _jsonSerializerOptions);
        }
    }
}