using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDPSystem.Entities
{
    [Table("CarEntities")]
    public class CarEntity : Car
    {
        public void Copy(CarEntity model)
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

        public ICollection<CarImage> CarImages { get; set; }
    }
}
