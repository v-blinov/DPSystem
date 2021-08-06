using System;

namespace ApiDPSystem.Entities
{
    public class CarImage
    {
        public Guid CarActualId { get; set; }
        public int ImageId { get; set; }

        public CarActual CarActual { get; set; }
        public Image Image { get; set; }
    }
}