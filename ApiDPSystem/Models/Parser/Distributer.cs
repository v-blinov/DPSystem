using ApiDPSystem.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;

namespace ApiDPSystem.Models.Parser
{
    public class Distributer
    {
        public Version GetJsonVersion(IFormFile file)
        {
            string jsonContent;

            using (var reader = new StreamReader(file.OpenReadStream()))
                jsonContent = reader.ReadToEnd();

            return JsonSerializer.Deserialize<Version>(jsonContent);
        }
    }
}
