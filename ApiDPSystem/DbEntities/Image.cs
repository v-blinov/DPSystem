using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.DbEntities
{
    public class Image
    {
        public int Id { get; set; }
        
        [Url]
        public string Url { get; set; }
        public int CarColorId { get; set; }
    }
}
