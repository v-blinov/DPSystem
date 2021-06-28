using ApiDPSystem.Data;
using ApiDPSystem.Models;
using ApiDPSystem.Records;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ApiDPSystem.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private Context _context;

        public delegate Task RegisterHandler(User user, string subject, string message);
        public event RegisterHandler SendMessage;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
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

        public async Task<User> FindUserByEmail(string email) => await _userManager.FindByEmailAsync(email);

        public AuthenticationResult GenerateJWTToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim("Name", user.FirstName + " " + user.LastName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                IsRevoked = false,
                Token = RandomString(25)
            };

            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            return new AuthenticationResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                Success = true
            };
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
