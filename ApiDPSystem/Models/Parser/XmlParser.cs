using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ApiDPSystem.Models.Parser
{
    public class XmlParser<T> : IParser<T> where T : FileFormat.ICar
    {
        public Root<T> DeserializeFile(string fileContent)
        {
            var serializer = new XmlSerializer(typeof(Root<T>));

            using var reader = new StringReader(fileContent);
            return (Root<T>)serializer.Deserialize(reader);
        }

        public List<Car> MapToDBModel(Root<T> deserializedModels)
        {
            throw new System.NotImplementedException();
        }
    }
}
