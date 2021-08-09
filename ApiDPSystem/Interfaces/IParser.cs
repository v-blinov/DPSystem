using ApiDPSystem.Models.Parser;
using System.Collections.Generic;

namespace ApiDPSystem.Interfaces
{
    public interface IParser<T> where T : IConvertableToDBCar
    {
        public Root<T> DeserializeFile(string fileContent);

        public List<Entities.CarActual> MapToDBModel(Root<T> deserializedModels, string dealer);
    }
}
