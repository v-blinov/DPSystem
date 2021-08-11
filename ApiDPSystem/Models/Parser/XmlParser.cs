using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.Models.Parser
{
    public class XmlParser : IParser
    {
        public string ConvertableFileExtension => ".xml";

        public List<CarActual> Parse(string fileContent, string fileName, string dealer)
        {
            var version = GetVersion(fileContent);
            var deserializedType = Selector.GetResultType(ConvertableFileExtension, version.Value);


            var serializer = new XmlSerializer(deserializedType);

            using var reader = new StringReader(fileContent);
            var deserializedModels = serializer.Deserialize(reader) as IRoot;

            var dbCars = deserializedModels.ConvertToActualDbModel(dealer);
            return dbCars;
        }

        public Version GetVersion(string fileContent)
        {
            var serializer = new XmlSerializer(typeof(Version));

            using var reader = new StringReader(fileContent);
            return (Version)serializer.Deserialize(reader);
        }
    }
}