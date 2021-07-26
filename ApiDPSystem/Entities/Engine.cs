using DataAnnotationsExtensions;
using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Engine
    {
        public int Id { get; set; }
        public string Fuel { get; set; }

        [Min(0)]
        public int? Power { get; set; }
        
        [Min(0)]
        public double? Capacity { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
