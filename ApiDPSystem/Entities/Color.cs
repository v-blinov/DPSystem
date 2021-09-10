using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }

        public ICollection<CarActual> InteriorCarActual { get; set; }
        public ICollection<CarActual> ExteriorCarActual { get; set; }

        public ICollection<CarHistory> InteriorCarHistory { get; set; }
        public ICollection<CarHistory> ExteriorCarHistory { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not Color color)
                return false;

            return color.HexCode == HexCode &&
                   color.Name == Name;
        }

        public override int GetHashCode() =>
            Name.GetHashCode() * 11
            + HexCode.GetHashCode() * 19;

        public Color GetValuesCopy() =>
             new ()
             {
                 Name = Name,
                 HexCode = HexCode
             };
    }
}