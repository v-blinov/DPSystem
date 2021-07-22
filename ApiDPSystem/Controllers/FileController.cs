using ApiDPSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class FileController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> ProcessJsonFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Log.Error("Файл не отправлен, или он пустой");
                return BadRequest();
            }

            try
            {
                string jsonContent = string.Empty;

                using (var reader = new StreamReader(file.OpenReadStream()))
                    jsonContent = await reader.ReadToEndAsync();

                var model = JsonSerializer.Deserialize<Root>(jsonContent);

                return Ok(model);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult ProcessXmlFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Log.Error("Файл не отправлен, или он пустой");
                return BadRequest();
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Root));

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var model = serializer.Deserialize(reader);
                    return Ok(model);
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
