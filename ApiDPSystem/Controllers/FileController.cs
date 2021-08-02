using ApiDPSystem.Models;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
        public async Task<ApiResponse> SendFileAsync(IFormFile file, [FromForm] string dealer)
        {
            var availableExtensions = new List<string> { ".json", ".xml", ".yaml", ".csv" };
            if (!availableExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                Log.Error($"Отправлен неподдерживаемый формат файла {Path.GetExtension(file.FileName)}");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Неподдерживаемый формат файла {Path.GetExtension(file.FileName)}",
                };
            }
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
            if (String.IsNullOrEmpty(dealer))
            {
                Log.Error("Имя диллера пустое или не отправлено.");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Имя диллера пустое или не отправлено.",
                };
            }

            try
            {
                // Написать реализацию обратного вызова,
                // возвращающего результат обработки файла для пользователя
                await _fileService.ProcessFileAsync(file, dealer);
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
