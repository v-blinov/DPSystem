using ApiDPSystem.Models.Parser;
using ApiDPSystem.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace ApiDPSystem.Services
{
    public class FileService : IFileService
    {
        private readonly IDataCheckerService _dataChecker;

        public FileService(IDataCheckerService dataCheckerService)
        {
            _dataChecker = dataCheckerService;
        }

        public async Task ProcessFileAsync(IFormFile file, string dealer)
        {
            var fileReadTask = ReadFileAsync(file);
            var fileExtension = Path.GetExtension(file.FileName);

            var parser = Selector.GetParser(fileExtension);

            var fileContent = await fileReadTask;
            var dDbModels = parser.Parse(fileContent, file.FileName, dealer);

            _dataChecker.TransferSoldCars(dDbModels, dealer);
            _dataChecker.SetToDatabase(dDbModels);
        }

        private static async Task<string> ReadFileAsync(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            return await reader.ReadToEndAsync();
        }
    }
}