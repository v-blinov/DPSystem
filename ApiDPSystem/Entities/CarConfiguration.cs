using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Entities
{
    public class CarConfiguration
    { 
        [Key]
        public Guid Id { get; set; }
        public string VinCode { get; set; }

        [Range(1900, 9999)]
        public int Year { get; set; }
        public string Model { get; set; }
        public string ModelTrim { get; set; }
        public decimal? Price { get; set; }
        public string Drive { get; set; }


        public int ProducerId { get; set; }
        public int EngineId { get; set; }
        public int TransmissionId { get; set; }
        public int InteriorColorId { get; set; }
        public int ExteriorColorId { get; set; }


        public Producer Producer { get; set; }
        public Engine Engine { get; set; }
        public Transmission Transmission { get; set; }
        public ICollection<ConfigurationFeature> ConfigurationFeatures { get; set; }
        public Color InteriorColor { get; set; }
        public Color ExteriorColor { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
