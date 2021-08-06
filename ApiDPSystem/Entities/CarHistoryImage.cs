using System;

namespace ApiDPSystem.Entities
{
    public class CarHistoryImage

    {
        public Guid CarHistoryId { get; set; }
        public int ImageId { get; set; }

        public CarHistory CarHistory { get; set; }
        public Image Image { get; set; }
    }
}