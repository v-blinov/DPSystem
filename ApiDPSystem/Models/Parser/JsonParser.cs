using System.Collections.Generic;
using System.Text.Json;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.Models.Parser
{
    public class JsonParser : IParser
    {
        public string ConvertableFileExtension => ".json";

        public List<CarActual> Parse(string fileContent, string fileName, string dealer)
        {
            var version = GetVersion(fileContent);
            var deserializedType = Selector.GetResultType(ConvertableFileExtension, version.Value);

            var deserializedModels = JsonSerializer.Deserialize(fileContent, deserializedType) as IRoot;

            var dbCars = deserializedModels!.ConvertToActualDbModel(dealer);

            return dbCars;
        }

        private Version GetVersion(string file) =>
            JsonSerializer.Deserialize<Version>(file);
    }
}