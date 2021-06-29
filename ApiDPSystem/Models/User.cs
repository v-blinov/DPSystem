using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Обязательное поле.")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Обязательное поле.")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
    }
}
