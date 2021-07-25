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
        public IActionResult SendFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Log.Error("Файл не отправлен, или он пустой");
                return BadRequest();
            }

            try
            {
                _fileService.ProcessFile(file);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
