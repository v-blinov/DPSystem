using System.Collections.Generic;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;
using YamlDotNet.Serialization;

namespace ApiDPSystem.Models.Parser
{
    public class YamlParser : IParser
    {
        public string ConvertableFileExtension => ".yaml";

        public List<CarActual> Parse(string fileContent, string fileName, string dealer)
        {
            var version = GetVersion(fileContent);
            var deserializedType = Selector.GetResultType(ConvertableFileExtension, version.Value);

            var deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
            var deserializedModels = deserializer.Deserialize(fileContent, deserializedType) as IRoot;

            var dbCars = deserializedModels.ConvertToActualDbModel(dealer);

            return dbCars;
        }

        public Version GetVersion(string fileContent)
        {
            var deserializer = new DeserializerBuilder()
                               .IgnoreUnmatchedProperties()
                               .Build();

            return deserializer.Deserialize<Version>(fileContent);
        }
    }
}