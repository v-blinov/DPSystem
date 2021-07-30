using System;

namespace ApiDPSystem.Entities
{
    public class ConfigurationFeature
    {
        public Guid CarId { get; set; }
        public int FeatureId { get; set; }

        public CarConfiguration CarConfiguration { get; set; }
        public Feature Feature { get; set; }
    }
}
