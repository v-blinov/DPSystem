using System;
using System.Collections.Generic;
using System.Linq;

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


        public override bool Equals(object obj)
        {
            if (obj is not Car car)
                return false;

            return car.VinCode == VinCode &&
                   car.Price == Price &&
                   car.IsSold == IsSold &&
                   car.IsActual == IsActual &&
                   car.Dealer.Equals(Dealer) &&
                   car.Configuration.Equals(Configuration) &&
                   car.InteriorColor.Equals(InteriorColor) &&
                   car.ExteriorColor.Equals(ExteriorColor) &&
                   car.CarImages.Count() == CarImages.Count() &&
                   car.CarImages.Intersect(CarImages).Count() == CarImages.Count() &&
                   car.CarFeatures.Count() == CarFeatures.Count() &&
                   car.CarFeatures.Intersect(CarFeatures).Count() == CarFeatures.Count();
        }

        public override int GetHashCode() =>
            VinCode.GetHashCode() * 11
            + IsSold.GetHashCode() * 19
            + IsActual.GetHashCode() * 7
            + Version * 88;

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