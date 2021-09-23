using System.Collections.Generic;
using System.Linq;
using ApiDPSystem.Entities;

namespace ApiDPSystem.Interfaces
{
    public interface IConvertableToDbCar
    {
        public Car ConvertCarToDbModel(string dealerName);

        public static List<CarFeature> GetFeaturesCollection(List<string> collection, string type)
        {
            if (collection == null)
                return new List<CarFeature>();

            return collection.Select(feature => new CarFeature { Feature = new Feature { Type = type, Description = feature } }).ToList();
        }
    }
}