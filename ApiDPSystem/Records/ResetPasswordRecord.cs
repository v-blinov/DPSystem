using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Records
{
    public record ResetPasswordRecord
    {
        public string Code { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"([0-9A-Za-z]{1}[\-0-9A-z\.]*[0-9A-Za-z]{1})@([A-Za-z\-]*\.){1,2}([A-Za-z]{1,3})", ErrorMessage = "Неверный формат логина.")]
        public string Email { get; init; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Поле \"Пароль\" должно иметь длину от 6 до 50 символов.")]
        [RegularExpression(@"^([0-9]+|[A-Z]+|[a-z]+|\S)*$", ErrorMessage = "Пароль должен содержать хотя бы одну большую и одну маленькую латинские буквы, спецсимвол и цифру")]
        public string Password { get; init; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; init; }

    }
}
