using ApiDPSystem.Models.Parser;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
            var dbModels = new List<Entities.Car>();

            switch (version.Value)
            {
                case 1:
                    var jsonParser_v1 = new JsonParser<FileFormat.Json.Version1.Car>();
                    var deserializeJsonModel = jsonParser_v1.DeserializeFile(fileContent);
                    dbModels = jsonParser_v1.MapToDBModel(deserializeJsonModel);
                    break;
                default:
                    throw new Exception($"Unknown Json file version {version.Value}");
            }
        }

        private void ProcessXmlWithVersion(string fileContent)
        {
            var version = new Distributer().XmlGetVersion(fileContent);
            var dbModels = new List<Entities.Car>();

            switch (version.Value)
            {
                case 1:
                    var xmlParser_v1 = new XmlParser<FileFormat.Xml.Version1.Car>();
                    var deserializeXmlModel = xmlParser_v1.DeserializeFile(fileContent);
                    dbModels = xmlParser_v1.MapToDBModel(deserializeXmlModel);
                    break;
                default:
                    throw new Exception($"Unknown Xml file version {version.Value}");
            }
        }

        private void ProcessYamlWithVersion(string fileContent)
        {
            var version = new Distributer().YamlGetVersion(fileContent);
            var dbModels = new List<Entities.Car>();

            switch (version.Value)
            {
                case 1:
                    var yamlParser_v1 = new YamlParser<FileFormat.Yaml.Version1.Car>();
                    var deserializeYamlModel = yamlParser_v1.DeserializeFile(fileContent);
                    dbModels = yamlParser_v1.MapToDBModel(deserializeYamlModel);
                    break;
                default:
                    throw new Exception($"Unknown Yaml file version {version.Value}");
            }
        }

        private void ProcessCsvWithVersion(string fileContent, string fileName) 
        {
            var version = new Distributer().CsvGetVersion(fileName);
            var dbModels = new List<Entities.Car>();

            switch (version.Value)
            {
                case 1:
                    var csvParser_v1 = new CsvParser<FileFormat.Csv.Version1.Car>();
                    var deserializeCsvModel_v1 = csvParser_v1.DeserializeFile_V1(fileContent);
                    dbModels = csvParser_v1.MapToDBModel(deserializeCsvModel_v1);
                    break;
                case 2:
                    var csvParser_v2 = new CsvParser<FileFormat.Csv.Version2.Car>();
                    var deserializeCsvModel_v2 = csvParser_v2.DeserializeFile_V2(fileContent);
                    dbModels = csvParser_v2.MapToDBModel(deserializeCsvModel_v2);
                    break;
                default:
                    throw new Exception($"Unknown Yaml file version {version.Value}");
            }
        }
    }
}
