using DataAnnotationsExtensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.DbEntities
{
    public class Car
    { 
        [Key]
        public Guid Id { get; set; }
        public string VinCode { get; set; }

        [Range(1900, 9999)]
        public int Year { get; set; }
        public string Model { get; set; }
        public string ModelTrim { get; set; }

        [Min(0, ErrorMessage = "Значение Price должно быть больше 0.")]
        public decimal? Price { get; set; }
        public string Drive { get; set; }


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
