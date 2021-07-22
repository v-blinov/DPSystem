using System.Collections.Generic;

namespace ApiDPSystem.DbEntities
{
    public class Transmission
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
