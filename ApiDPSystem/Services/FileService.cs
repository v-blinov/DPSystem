using ApiDPSystem.Models.Parser;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace ApiDPSystem.Services
{
    public class FileService
    {
        public void ProcessFile(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            switch (fileExtension)
            {
                case ".json":
                    ProcessJsonWithVersion(file);
                    break;
                case ".xml":
                    new XmlParser().ProcessFile(file);
                    break;
                case ".yaml":
                    new YamlParser().ProcessFile(file);
                    break;
                case ".csv":
                    new CsvParser().ProcessFile(file);
                    break;
                default:
                    throw new Exception("Неверный формат файла");
            }
        }

        public void ProcessJsonWithVersion(IFormFile file)
        {
            var version = new Distributer().GetJsonVersion(file);

            switch (version.VersionValue)
            {
                case 1:
                    var deserializeJsonModel = new JsonParser<FileFormat.Json.Version1.Car>().DeserializeFile(file);

                    break;
                default:
                    throw new Exception($"Unknown file version {version.VersionValue}");
            }
        }
    }
}
