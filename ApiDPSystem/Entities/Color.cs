using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }

        public ICollection<Car> InteriorCars { get; set; }
        public ICollection<Car> ExteriorCars { get; set; }
    }
}
