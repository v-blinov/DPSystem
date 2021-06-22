using ApiDPSystem.Models;
using ApiDPSystem.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Web;

namespace ApiDPSystem.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public delegate Task RegisterHandler(User user, string subject, string message);
        public event RegisterHandler SendMessage;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> LogIn(LogInRecord logInModel)
        {
            var checkResult = await CheckIfEmailConfirmedAsync(logInModel.Email);
            if (!checkResult.Succeeded)
                return checkResult;
            
            var signInResult = await _signInManager.PasswordSignInAsync(logInModel.Email, logInModel.Password, false, false);
            if (!signInResult.Succeeded)
                return IdentityResult.Failed(new IdentityError()
                {
                    Description = "Неправильный логин и(или) пароль."
                });

            return IdentityResult.Success;
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

        public async Task ForgotPassword(string email, string url)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            url = url.Replace("userIdValue", user.Id);
            url = url.Replace("codeValue", code);

            SendMessage?.Invoke(user, "Reset password", $"Для сброса пароля пройдите по ссылке: <a href='{url}'>link</a>");
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordRecord resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);

            if (user == null)
                return IdentityResult.Failed(new IdentityError()
                {
                    Description = $"Пользователь с email {resetPassword.Email} не найден."
                });

            return await _userManager.ResetPasswordAsync(user, resetPassword.Code, resetPassword.Password);
        }

        public async Task<IdentityResult> CheckIfEmailConfirmedAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return IdentityResult.Failed(new IdentityError()
                {
                    Description = $"Пользователь с email {email} не найден."
                });

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return IdentityResult.Failed(new IdentityError()
                {
                    Description = $"Email {email} не подтвержден."
                });

            return IdentityResult.Success;
        }
    }
}
