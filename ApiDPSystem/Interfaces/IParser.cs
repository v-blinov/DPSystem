using ApiDPSystem.Models.Parser;
using System;
using System.Collections.Generic;

namespace ApiDPSystem.Interfaces
{
    public interface IParser<T> where T : FileFormat.ICar
    {
        public Root<T> DeserializeFile(string fileContent);

        //public List<Entities.CarEntity> MapToDBModel(Root<T> deserializedModels)
        //{
        //    var dbCars = new List<Entities.CarEntity>();

        //    foreach (var deserializeModel in deserializedModels.Cars)
        //        dbCars.Add(new Entities.CarEntity
        //        {
        //            CarConfiguration = deserializeModel.ConvertToDbModel(),
        //            Price = Decimal.TryParse(deserializeModel. Price, out decimal price) ? price : null,

        //        });

        //    return dbCars;
        //}
    }
}
