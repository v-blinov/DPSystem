using System;

namespace ApiDPSystem.Entities
{
    public class ConfigurationFeature
    {
        public Guid ConfigurationId { get; set; }
        public int FeatureId { get; set; }

        public Configuration Configuration { get; set; }
        public Feature Feature { get; set; }
    }
}
