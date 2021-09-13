using System;
using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Car
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string VinCode { get; set; }
        public decimal? Price { get; set; }
        public bool IsSold { get; set; }
        public bool IsActual { get; set; }


        public int DealerId { get; set; }
        public Guid ConfigurationId { get; set; }
        public int ExteriorColorId { get; set; }
        public int InteriorColorId { get; set; }


        public Dealer Dealer { get; set; }
        public Configuration Configuration { get; set; }
        public Color ExteriorColor { get; set; }
        public Color InteriorColor { get; set; }
        public IEnumerable<CarFeature> CarFeatures { get; set; }
        public IEnumerable<CarImage> CarImages { get; set; }
        
        public Car Copy() =>
            new()
            {
                VinCode = VinCode,
                Price = Price,
                IsSold = IsSold,
                IsActual = IsActual,
                Dealer = Dealer,
                CarFeatures = CopyCarFeatureList(),
                CarImages = CopyCarImageList(),
                Configuration = Configuration.GetValuesCopy(),
                ExteriorColor = ExteriorColor.GetValuesCopy(),
                InteriorColor = InteriorColor.GetValuesCopy()
            };

        private List<CarFeature> CopyCarFeatureList()
        {
            var copy = new List<CarFeature>();

            foreach (var carFeature in CarFeatures)
                copy.Add(carFeature.GetValuesCopy());

            return copy;
        }

        private List<CarImage> CopyCarImageList()
        {
            var copy = new List<CarImage>();

            foreach (var carImage in CarImages)
                copy.Add(carImage.GetValuesCopy());

            return copy;
        }
    }
}