using System.Collections.Generic;
using ApiDPSystem.Entities;
using ApiDPSystem.Models.Parser;

namespace ApiDPSystem.Interfaces
{
    public interface IParser<T> where T : IConvertableToDBCar
    {
        public Root<T> DeserializeFile(string fileContent);

        public List<CarActual> MapToDBModel(Root<T> deserializedModels, string dealer);
    }
}