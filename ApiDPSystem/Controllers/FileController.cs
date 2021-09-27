using System;
using System.Threading.Tasks;
using ApiDPSystem.Exceptions;
using ApiDPSystem.Filters;
using ApiDPSystem.Models;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
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
                    Message = ex.Message
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
        
        
        /// <summary>
        /// Create file with cars info for certain dealer by any condition
        /// FileFormat: 0,1 - json | 2 - xml | 3 - yaml | 4 - csv
        /// Category: 0,1 - all | 2 - sold | 3 - actual
        /// </summary>
        /// <param name="filter">
        /// Category:
        ///     0 - all cars for dealer
        ///     1 - all filters
        ///     2 - only sold cars
        ///     3 - only actual cars
        /// Dealer Name
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFileWithCarsByFilterInfo([FromForm] Filter filter)
        {
            if (filter is null)
            {
                Log.Error("При обращении к методу GetCarsByFilterInCsv filter - null");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            if (string.IsNullOrEmpty(filter.DealerName))
            {
                Log.Warning("При обращении к методу GetCarsByFilterInCsv не отправлен обязательный параметр dealerName");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            if (filter.FileFormat == Models.FileFormat.unknown)
            {
                filter.FileFormat = Models.FileFormat.json;
                Log.Warning("Формат файла для метода десериализации данных не задан, по умолчанию будет использоваться формат .json");
            }
            
            try
            {
                var fileName = $"{filter.DealerName}_{Enum.GetName(filter.Category)}.{filter.FileFormat.GetDisplayName()}";
                return _fileService.CreateFile(fileName, filter);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}