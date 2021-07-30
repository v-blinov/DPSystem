using System;
using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Entities
{
    public class Image
    {
        public int Id { get; set; }
        
        [Url]
        public string Url { get; set; }
        public Guid CarId { get; set; }

        public CarConfiguration Configurations { get; set; }
    }
}
