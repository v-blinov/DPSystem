using System.Collections.Generic;

namespace ApiDPSystem.FileFormat
{
    public interface ICar
    {
        public Entities.CarActual ConvertToCarActualDbModel(string DealerName);

        public static List<Entities.ConfigurationFeature> GetFeaturesCollection(List<string> collection, string type)
        {
            var features = new List<Entities.ConfigurationFeature>();

            if (collection != null)
            {
                foreach (var feature in collection)
                    features.Add(new Entities.ConfigurationFeature { Feature = new Entities.Feature { Type = type, Description = feature } });
            }

            return features;
        }
    }
}
