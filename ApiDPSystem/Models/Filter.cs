using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Models
{
    public enum Category
    {
        Disabled = 0,
        All = 1,
        IsSold = 2,
        IsActual = 3
    }
    
    public class Filter
    {
        public Category Category { get; set; }
        
        public string DealerName { get; set; }
    }
}