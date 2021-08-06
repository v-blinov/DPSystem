using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDPSystem.Entities
{
    [Table("CarHistories")]
    public class CarHistory : Car
    {
        public bool IsSold { get; set; }
        public ICollection<CarHistoryImage> CarHistoryImages { get; set; }


        public void Copy(CarActual model)
        {
            VinCode = model.VinCode;
            Price = model.Price;
            DealerId = model.DealerId;
            ConfigurationId = model.ConfigurationId;
            InteriorColorId = model.InteriorColorId;
            ExteriorColorId = model.ExteriorColorId;

            CarHistoryImages = new List<CarHistoryImage>();
        }
    }
}
