using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ApiDPSystem.Services.Interface
{
    public interface IFileService
    {
        public Task ProcessFileAsync(IFormFile file, string dealer);
        //public Task<string> ReadFileAsync(IFormFile file);
    }
}
