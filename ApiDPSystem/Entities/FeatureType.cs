using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class FeatureType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Feature> Features { get; set; }
    }
}
