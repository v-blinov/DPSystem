using System;

namespace ApiDPSystem.Entities
{
    public class CarImage
    {
        public Guid CarEntityId { get; set; }
        public int ImageId { get; set; }

        public CarEntity CarEntity { get; set; }
        public Image Image { get; set; }
    }
}