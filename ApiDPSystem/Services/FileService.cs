using ApiDPSystem.Models.Parser;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using ApiDPSystem.Services.Interfaces;

namespace ApiDPSystem.Services
{
    public class FileService
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
    }
}