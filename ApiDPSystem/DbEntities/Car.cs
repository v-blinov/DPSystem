using DataAnnotationsExtensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.DbEntities
{
    public class Car
    { 
        public int Id { get; set; }
        public string VinCode { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Year { get; set; }
        public string Model { get; set; }
        public string ModelTrim { get; set; }

        [Min(0, ErrorMessage = "Значение Price должно быть больше 0.")]
        public decimal? Price { get; set; }


        public int ProducerId { get; set; }
        public int EngineId { get; set; }
        public int TransmissionId { get; set; }


        public Producer Producer { get; set; }
        public Engine Engine { get; set; }
        public Transmission Transmission { get; set; }
        public CarColor CarColor { get; set; }
        public CarFeature CarFeature { get; set; }
    }
}
