using System.Collections.Generic;

namespace ApiDPSystem.DbEntities
{
    public class Color
    {
        public int Id { get; set; }
        public string HexCode { get; set; }
        
        public ICollection<CarColor> CarColors { get; set; }
    }
}
