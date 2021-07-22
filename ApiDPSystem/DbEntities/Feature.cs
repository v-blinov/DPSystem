using System.Collections.Generic;

namespace ApiDPSystem.DbEntities
{
    public class Feature
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int FeatureTypeId { get; set; }

        public ICollection<FeatureType> FeatureTypes { get; set; }
        public CarFeature CarFeature { get; set; }
    }
}
