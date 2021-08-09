using System.Collections.Generic;
using ApiDPSystem.Entities;

namespace ApiDPSystem.Interfaces
{
    public interface IConvertableToDBCar
    {
        public CarActual ConvertToCarActualDbModel(string DealerName);

        public static List<ConfigurationFeature> GetFeaturesCollection(List<string> collection, string type)
        {
            var features = new List<ConfigurationFeature>();

            if (collection != null)
                foreach (var feature in collection)
                    features.Add(new ConfigurationFeature {Feature = new Feature {Type = type, Description = feature}});

            return features;
        }
    }
}