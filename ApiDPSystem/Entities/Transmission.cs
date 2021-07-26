using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Transmission
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
