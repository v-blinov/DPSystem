using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Producer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Configuration> Configurations { get; set; }
    }
}
