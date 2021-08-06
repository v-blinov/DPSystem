using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace ApiDPSystem.Models.Parser
{
    public class YamlParser<T> : IParser<T> where T : FileFormat.ICar
    {
        public Root<T> DeserializeFile(string fileContent)
        {
            var deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
            return deserializer.Deserialize<Root<T>>(fileContent);
        }

        public List<CarActual> MapToDBModel(Root<T> deserializedModels, string dealer)
        {
            var dbCars = new List<CarActual>();

            foreach (var deserializeModel in deserializedModels.Cars)
                dbCars.Add(deserializeModel.ConvertToCarActualDbModel(dealer));

            return dbCars;
        }
    }
}
