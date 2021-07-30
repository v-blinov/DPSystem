using System.Collections.Generic;

namespace ApiDPSystem.FileFormat
{
    public interface ICar
    {
        public Entities.CarConfiguration ConvertToDbModel();

        public static List<Entities.ConfigurationFeature> GetFeaturesCollection(List<string> collection, string featureType)
        {
            var features = new List<Entities.ConfigurationFeature>();

            if (collection != null)
            {
                foreach (var feature in collection)
                    features.Add(new Entities.ConfigurationFeature { Feature = new Entities.Feature { FeatureType = new Entities.FeatureType { Name = featureType }, Description = feature } });
            }

            return features;
        }
    }
}
