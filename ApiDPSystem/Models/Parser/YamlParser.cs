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

        public List<CarEntity> MapToDBModel(Root<T> deserializedModels)
        {
            throw new System.NotImplementedException();
        }

        //public List<Entities.CarConfiguration> MapToDBModel(Root<T> deserializedModels)
        //{
        //    var dbCars = new List<Entities.CarConfiguration>();

        //    foreach (var deserializeModel in deserializedModels.Cars)
        //        dbCars.Add(deserializeModel.ConvertToDbModel());

        //    return dbCars;
        //}
    }
}
