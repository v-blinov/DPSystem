using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using System.Text.Json;

namespace ApiDPSystem.Models.Parser
{
    public class JsonParser<T> : IParser<T> where T : FileFormat.ICar
    {
        public Root<T> DeserializeFile(string fileContent) =>
            JsonSerializer.Deserialize<Root<T>>(fileContent);

        public List<CarActual> MapToDBModel(Root<T> deserializedModels, string dealer)
        {
            var dbCars = new List<CarActual>();

            foreach (var deserializeModel in deserializedModels.Cars)
                dbCars.Add(deserializeModel.ConvertToCarActualDbModel(dealer));

            return dbCars;
        }
    }
}
