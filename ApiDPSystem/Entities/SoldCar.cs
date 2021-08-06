using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDPSystem.Entities
{
    [Table("SoldCars")]
    public class SoldCar : Car
    {
        public void Copy(CarEntity model)
        {
            VinCode = model.VinCode;
            Price = model.Price;
            DealerId = model.DealerId;
            ConfigurationId = model.ConfigurationId;
            InteriorColorId = model.InteriorColorId;
            ExteriorColorId = model.ExteriorColorId;

            SoldCarImages = new List<SoldCarImage>();
        }

        public ICollection<SoldCarImage> SoldCarImages { get; set; }
    }
}
