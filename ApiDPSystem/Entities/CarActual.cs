using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ApiDPSystem.Entities
{
    [Table("CarActuals")]
    public class CarActual : Car
    {
        public ICollection<CarImage> CarImages { get; set; }

        public CarActual GetValuesCopy() =>
            new ()
            {
                VinCode = VinCode,
                Price = Price,
                Dealer = Dealer,
                Configuration = Configuration.GetValuesCopy(),
                ExteriorColor = ExteriorColor.GetValuesCopy(),
                InteriorColor = InteriorColor.GetValuesCopy(),
                CarImages = CloneCarImagesList(),
            };

        private List<CarImage> CloneCarImagesList()
        {
            var ci_clone = new List<CarImage>();

            CarImages
                .ToList()
                .ForEach(p => ci_clone.Add(p.GetValuesCopy()));

            return ci_clone;
        }
    }
}