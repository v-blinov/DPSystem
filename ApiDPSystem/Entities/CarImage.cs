using System;

namespace ApiDPSystem.Entities
{
    public class CarImage
    {
        public int CarEntityId { get; set; }
        public Guid ImageId { get; set; }

        public CarEntity CarEntity { get; set; }
        public Image Image { get; set; }
    }
}