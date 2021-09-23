using System;
using System.Threading.Tasks;
using ApiDPSystem.Exceptions;
using ApiDPSystem.Filters;
using ApiDPSystem.Models;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public ActionResult GetActualCarsInJsonFile([FromForm] string dealerName= "Izhevsk")
        {
            if (string.IsNullOrEmpty(dealerName))
            {
                Log.Warning("При обращении к методу GetActualCarsInJsonFile не отправлен обязательный параметр dealerName");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            try
            {
                var fileName = $"{dealerName}_Actual.json";
                Func<string, string> getCarsMethod = _fileService.GetActualCarsInStringAsJson;

                return _fileService.CreateJsonFile(getCarsMethod, fileName, dealerName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost]
        public ActionResult GetSoldCarsInJsonFile([FromForm] string dealerName = "Izhevsk")
        {
            if (string.IsNullOrEmpty(dealerName))
            {
                Log.Warning("При обращении к методу GetSoldCarsInJsonFile не отправлен обязательный параметр dealerName");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            try
            {
                var fileName = $"{dealerName}_Sold.json";
                Func<string, string> getCarsMethod = _fileService.GetSoldCarsInStringAsJson;

                return _fileService.CreateJsonFile(getCarsMethod, fileName, dealerName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost]
        public ActionResult GetAllHistoryInJsonFile([FromForm] string dealerName= "Izhevsk")
        {
            if (string.IsNullOrEmpty(dealerName))
            {
                Log.Warning("При обращении к методу GetAllHistoryInJsonFile не отправлен обязательный параметр dealerName");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            try
            {
                var fileName = $"{dealerName}_History.json";
                Func<string, string> getCarsMethod = _fileService.GetAllHistoryInStringAsJson;

                return _fileService.CreateJsonFile(getCarsMethod, fileName, dealerName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}