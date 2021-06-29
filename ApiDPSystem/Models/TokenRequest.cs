using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Models
{
    public class TokenRequest
    {
        [Required(ErrorMessage = "Обязательное поле.")]
        public string Token { get; set; }
        [Required(ErrorMessage = "Обязательное поле.")]
        public string RefreshToken { get; set; }
    }
}
