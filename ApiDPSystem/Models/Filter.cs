using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Models
{
    public enum Category
    {
        Disabled = 0,
        All = 1,
        Sold = 2,
        Actual = 3
    }

    public enum FileFormat
    {
        unknown = 0,
        json = 1,
        xml = 2,
        yaml = 3,
        csv = 4
    }
    
    public class Filter
    {
        public string DealerName { get; set; }
        public FileFormat FileFormat { get; set; }
        public Category Category { get; set; }
    }
}