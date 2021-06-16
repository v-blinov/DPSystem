using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Records
{
    public class ForgotPasswordRecord
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Обязательное поле.")]
        public string Email { get; init; }
    }
}
