using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Records
{
    public record LogInRecord
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Обязательное поле.")]
        public string Email { get; init; }


        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Обязательное поле.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль должен иметь длину от 6 до 50 символов.")]
        public string Password { get; init; }
    }
}
