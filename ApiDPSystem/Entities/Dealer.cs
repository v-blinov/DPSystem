using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Dealer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CarEntity> CarEntities { get; set; }
    }
}
