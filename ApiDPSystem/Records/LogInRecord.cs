using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Records
{
    public class LogInRecord
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Email")]
        public string Email { get; init; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; init; }
    }
}
