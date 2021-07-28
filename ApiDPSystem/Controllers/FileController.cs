using ApiDPSystem.Models;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class FileController : Controller
    {
        private readonly FileService _fileService;
        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }


        [HttpPost]
        public ApiResponse SendFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Log.Error("Файл не отправлен, или он пустой");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Файл не отправлен, или он пустой.",
                };
            }

            try
            {
                _fileService.ProcessFileAsync(file);
                return new ApiResponse()
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }
    }
}
