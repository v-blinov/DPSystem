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

        public List<Entities.CarConfiguration> MapToDBModel(Root<T> deserializedModels)
        {
            var dbCars = new List<Entities.CarConfiguration>();

            foreach (var deserializeModel in deserializedModels.Cars)
                dbCars.Add(deserializeModel.ConvertToDbModel());

            return dbCars;
        }
    }
}
