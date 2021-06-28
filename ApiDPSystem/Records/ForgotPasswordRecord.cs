using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Records
{
    public class ForgotPasswordRecord
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Обязательное поле.")]
        public string Email { get; init; }
    }
}
