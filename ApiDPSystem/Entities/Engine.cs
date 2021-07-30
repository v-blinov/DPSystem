using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Engine
    {
        public int Id { get; set; }
        public string Fuel { get; set; }
        public int? Power { get; set; }
        public double? Capacity { get; set; }

        public ICollection<CarConfiguration> Configurations { get; set; }
    }
}
