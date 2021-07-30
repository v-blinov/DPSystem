using ApiDPSystem.Interfaces;
using ApiDPSystem.Models.Parser;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApiDPSystem.Services
{
    public class FileService
    {
        public async Task ProcessFileAsync(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);

            var fileContent = await ReadFileAsync(file);

            switch (fileExtension)
            {
                case ".json":
                    ProcessJsonWithVersion(fileContent);
                    break;
                case ".xml":
                    ProcessXmlWithVersion(fileContent);
                    break;
                case ".yaml":
                    ProcessYamlWithVersion(fileContent);
                    break;
                case ".csv":
                    ProcessCsvWithVersion(fileContent, file.FileName);
                    break;
                default:
                    throw new Exception("Unknown file format");
            }
        }

        private async Task<string> ReadFileAsync(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            return await reader.ReadToEndAsync();
        }

        private void ProcessJsonWithVersion(string fileContent)
        {
            var version = new Distributer().JsonGetVersion(fileContent);

            switch (version.Value)
            {
                case 1:
                    var jsonParser = new JsonParser<FileFormat.Json.Version1.Car>();
                    var deserializeJsonModel = jsonParser.DeserializeFile(fileContent);
                    var dbModels = jsonParser.MapToDBModel(deserializeJsonModel);
                    break;
                default:
                    throw new Exception($"Unknown Json file version {version.Value}");
            }
        }

        private void ProcessXmlWithVersion(string fileContent)
        {
            var version = new Distributer().XmlGetVersion(fileContent);

            switch (version.Value)
            {
                case 1:
                    var deserializeXmlModel = new XmlParser<FileFormat.Xml.Version1.Car>().DeserializeFile(fileContent);

                    break;
                default:
                    throw new Exception($"Unknown Xml file version {version.Value}");
            }
        }

        private void ProcessYamlWithVersion(string fileContent)
        {
            var version = new Distributer().YamlGetVersion(fileContent);

            switch (version.Value)
            {
                case 1:
                    var deserializeYamlModel = new YamlParser<FileFormat.Yaml.Version1.Car>().DeserializeFile(fileContent);
                    
                    break;
                default:
                    throw new Exception($"Unknown Yaml file version {version.Value}");
            }
        }

        private void ProcessCsvWithVersion(string fileContent, string fileName) 
        {
            var version = new Distributer().CsvGetVersion(fileName);

            switch (version.Value)
            {
                case 1:
                    var deserializeCsvModel_v1 = new CsvParser<FileFormat.Csv.Version1.Car>().DeserializeFile_Version_1(fileContent);
                    break;
                case 2:
                    var deserializeCsvModel_v2 = new CsvParser<FileFormat.Csv.Version2.Car>().DeserializeFile_Version_2(fileContent);
                    break;
                default:
                    throw new Exception($"Unknown Yaml file version {version.Value}");
            }
        }
    }
}
