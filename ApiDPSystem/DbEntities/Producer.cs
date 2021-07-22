using System.Collections.Generic;

namespace ApiDPSystem.DbEntities
{
    public class Producer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
