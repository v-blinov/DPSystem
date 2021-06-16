using ApiDPSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Web;

namespace ApiDPSystem.Services
{
    public class RegisterService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;

        public delegate Task RegisterHandler(User user, string subject, string message);
        public event RegisterHandler SendMessage;

        public RegisterService(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<bool> Register(User user, string password, string url)
        {
            var response = await userManager.CreateAsync(user, password);
            if (response.Succeeded)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                url = url.Replace("userIdValue", user.Id);
                url = url.Replace("codeValue", HttpUtility.UrlEncode(code));

                SendMessage?.Invoke(user, "Confirm your account", $"Подтвердите регистрацию, перейдя по ссылке: <a href='{url}'>Confirm your email</a>");

                return true;
            }
            return false;
        }
        
        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return false;

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded;
        }
    }
}
