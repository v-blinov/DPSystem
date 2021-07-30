using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Feature
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int FeatureTypeId { get; set; }

        public FeatureType FeatureType { get; set; }
        public ICollection<ConfigurationFeature> ConfigurationFeature { get; set; }
    }
}
