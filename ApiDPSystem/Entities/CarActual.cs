using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDPSystem.Entities
{
    [Table("CarActuals")]
    public class CarActual : Car
    {
        public ICollection<CarImage> CarImages { get; set; }

        public void Copy(CarActual model)
        {
            VinCode = model.VinCode;
            CarImages = model.CarImages;
            Price = model.Price;

            if (model.Configuration != null) Configuration = model.Configuration;
            else ConfigurationId = model.ConfigurationId;

            if (model.Dealer != null) Dealer = model.Dealer;
            else DealerId = model.DealerId;

            if (model.ExteriorColor != null) ExteriorColor = model.ExteriorColor;
            else ExteriorColorId = model.ExteriorColorId;

            if (model.InteriorColor != null) InteriorColor = model.InteriorColor;
            else InteriorColorId = model.InteriorColorId;
        }
    }
}