using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Entities
{
    public class Image
    {
        public int Id { get; set; }
        
        [Url]
        public string Url { get; set; }

        public ICollection<CarImage> CarImages { get; set; }
    }
}
