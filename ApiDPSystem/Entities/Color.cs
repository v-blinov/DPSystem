using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }

        public ICollection<CarEntity> InteriorCarEntity { get; set; }
        public ICollection<CarEntity> ExteriorCarEntity { get; set; }
    }
}
