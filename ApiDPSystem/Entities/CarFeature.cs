using System;

namespace ApiDPSystem.Entities
{
    public class CarFeature
    {
        public Guid CarId { get; set; }
        public int FeatureId { get; set; }

        public Car Car { get; set; }
        public Feature Feature { get; set; }
    }
}
