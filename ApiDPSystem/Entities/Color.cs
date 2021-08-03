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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is not Color color)
                return false;

            return color.HexCode == HexCode &&
                   color.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 11
                 + HexCode.GetHashCode() * 19;
        }
    }
}
