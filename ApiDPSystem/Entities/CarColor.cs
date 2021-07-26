using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class CarColor
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int ColorInteriorId { get; set; }
        public int ColorExteriorId { get; set; }

        public ICollection<Car> Cars { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
