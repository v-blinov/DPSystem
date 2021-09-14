using System;

namespace ApiDPSystem.Entities
{
    public class CarImage
    {
        public Guid CarId { get; set; }
        public int ImageId { get; set; }

        public Car Car { get; set; }
        public Image Image { get; set; }

        public CarImage GetValuesCopy() =>
            new()
            {
                Image = Image.GetValuesCopy()
            };
    }
}