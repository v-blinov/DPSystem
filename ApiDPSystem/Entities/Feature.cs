using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Feature
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public ICollection<ConfigurationFeature> ConfigurationFeature { get; set; }
    }
}
