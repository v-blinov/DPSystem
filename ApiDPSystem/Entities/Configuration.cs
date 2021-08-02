using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Entities
{
    public class Configuration
    {
        public Guid Id { get; set; }

        [Range(1900, 9999)]
        public int Year { get; set; }
        public string Model { get; set; }
        public string ModelTrim { get; set; }
        public string Transmission { get; set; }
        public string Drive { get; set; }


        public int ProducerId { get; set; }
        public int EngineId { get; set; }


        public ICollection<CarEntity> CarEntities { get; set; } 
        public ICollection<ConfigurationFeature> ConfigurationFeatures { get; set; }
        public Producer Producer { get; set; }
        public Engine Engine { get; set; }
    }
}
