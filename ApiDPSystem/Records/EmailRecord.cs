using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Records
{
    public record EmailRecord
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Обязательное поле.")]
        [RegularExpression(@"([0-9A-Za-z]{1}[\-0-9A-z\.]*[0-9A-Za-z]{1})@([A-Za-z\-]*\.){1,2}([A-Za-z]{1,3})", ErrorMessage = "Неверный формат почты.")]
        public string Email { get; init; }
    }
}