using System;
using System.Threading.Tasks;
using System.Xml;
using ApiDPSystem.Exceptions;
using ApiDPSystem.Filters;
using ApiDPSystem.Models;
using ApiDPSystem.Services;
using ApiDPSystem.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

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
        [FileSendParamsValidationFilter]
        public async Task<ApiResponse> SendFileAsync(IFormFile file, [FromForm] string dealer = "Izhevsk")
        {
            try
            {
                await _fileService.ProcessFileAsync(file, dealer);
                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (InvalidFileVersionException ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message ?? "Неподдерживаемая версия файла данного формата"
                };
            }
            catch (InvalidFileException ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Некорректное содержимое файла"
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }
    }
}