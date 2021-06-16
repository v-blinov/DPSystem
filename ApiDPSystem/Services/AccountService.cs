using ApiDPSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Web;

namespace ApiDPSystem.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;

        public delegate Task RegisterHandler(User user, string subject, string message);
        public event RegisterHandler SendMessage;

        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Register(User user, string password, string url)
        {
            var response = await _userManager.CreateAsync(user, password);
            if (response.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                url = url.Replace("userIdValue", user.Id);
                url = url.Replace("codeValue", HttpUtility.UrlEncode(code));

                SendMessage?.Invoke(user, "Confirm your account", $"Подтвердите регистрацию, перейдя по ссылке: <a href='{url}'>Confirm your email</a>");
            }
            return response;
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded;
        }

        public async Task ForgotPassword(User user, string url)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            url = url.Replace("userIdValue", user.Id);
            url = url.Replace("codeValue", code);

            SendMessage?.Invoke(user, "Reset password", $"Для сброса пароля пройдите по ссылке: <a href='{url}'>link</a>");
        }
    }
}
