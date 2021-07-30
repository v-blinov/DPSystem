using ApiDPSystem.Models.Parser;
using System.Collections.Generic;

namespace ApiDPSystem.Interfaces
{
    public interface IParser<T> where T : FileFormat.ICar
    {
        public Root<T> DeserializeFile(string fileContent);

        public List<Entities.Car> MapToDBModel(Root<T> deserializedModels)
        {
            var dbCars = new List<Entities.Car>();

            foreach (var deserializeModel in deserializedModels.Cars)
                dbCars.Add(deserializeModel.ConvertToDbModel());

            return dbCars;
        }
    }
}
