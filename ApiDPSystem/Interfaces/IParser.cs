using ApiDPSystem.Models.Parser;
using System;
using System.Collections.Generic;

namespace ApiDPSystem.Interfaces
{
    public interface IParser<T> where T : FileFormat.ICar
    {
        public Root<T> DeserializeFile(string fileContent);

        public List<Entities.CarActual> MapToDBModel(Root<T> deserializedModels, string dealer);
    }
}
