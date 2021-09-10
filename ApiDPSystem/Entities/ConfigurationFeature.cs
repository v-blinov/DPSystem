using System;

namespace ApiDPSystem.Entities
{
    public class ConfigurationFeature
    {
        public Guid ConfigurationId { get; set; }
        public int FeatureId { get; set; }

        public Configuration Configuration { get; set; }
        public Feature Feature { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not ConfigurationFeature configurationFeature)
                return false;

            return configurationFeature.Configuration.Equals(Configuration) &&
                   configurationFeature.Feature.Equals(Feature);
        }

        public override int GetHashCode() =>
            Configuration.GetHashCode() * 31 + Feature.GetHashCode() * 17;

        public ConfigurationFeature GetValuesCopy() =>
            new()
            {
                Feature = Feature.GetValuesCopy()
            };
    }
}