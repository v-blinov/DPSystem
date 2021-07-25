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
                    new JsonParser().ProcessFile(file);
                    break;
                case ".xml":
                    new XmlParser().ProcessFile(file);
                    break;
                //case ".yaml":
                //    new JsonParser().ProcessFile(file);
                //    break;
                //case ".csv":
                //    new JsonParser().ProcessFile(file);
                //    break;
                default:
                    throw new Exception("Неверный формат файла");
            }
        }
    }
}
