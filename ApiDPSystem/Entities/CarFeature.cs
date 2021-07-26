using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class CarFeature
    {
        public int CarId { get; set; }
        public int FeatureId { get; set; }

        public ICollection<Car> Cars { get; set; }
        public ICollection<Feature> Features { get; set; }
    }
}
