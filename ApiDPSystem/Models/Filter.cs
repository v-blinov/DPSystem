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
    
    public class Filter
    {
        public Category Category { get; set; }

        public string DealerName { get; set; } = "Izhevsk";
    }
}