using System;
using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class CarEntity
    {
        public Guid Id { get; set; }
        public string VinCode { get; set; }
        public decimal? Price { get; set; }
        public bool IsSold { get; set; }


        public int DealerId { get; set; }
        public Guid ConfigurationId { get; set; }
        public int ExteriorColorId { get; set; }
        public int InteriorColorId { get; set; }


        public Dealer Dealer { get; set; }
        public Configuration Configuration { get; set; }
        public Color ExteriorColor { get; set; }
        public Color InteriorColor { get; set; }
        public ICollection<CarImage> CarImages {get; set;}
    }
}
