using System.Collections.Generic;

namespace ApiDPSystem.FileFormat
{
    public interface ICar
    {
        public Entities.Car ConvertToDbModel();

        public static List<Entities.CarFeature> GetFeaturesCollection(List<string> collection, string featureType)
        {
            var features = new List<Entities.CarFeature>();

            if (collection != null)
            {
                foreach (var feature in collection)
                    features.Add(new Entities.CarFeature { Feature = new Entities.Feature { FeatureType = new Entities.FeatureType { Name = featureType }, Description = feature } });
            }

            return features;
        }
    }
}
