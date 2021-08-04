using System;

namespace ApiDPSystem.Entities
{
    public class SoldCarImage

    {
        public Guid SoldCarId { get; set; }
        public int ImageId { get; set; }

        public SoldCar SoldCar { get; set; }
        public Image Image { get; set; }
    }
}