using ApiDPSystem.Data;
using ApiDPSystem.Models;
using ApiDPSystem.Records;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ApiDPSystem.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IConfiguration _configuration;
        private Context _context;
        public delegate Task RegisterHandler(User user, string subject, string message);
        public event RegisterHandler SendMessage;

        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IConfiguration configuration,
                              Context context,
                              TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
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

            var refreshTokenInfo = new RefreshTokenInfo()
            {
                JwtId = token.Id,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                RefreshToken = GetRandomString()
            };

            _context.RefreshTokenInfoTable.Add(refreshTokenInfo);
            _context.SaveChanges();

            return new AuthenticationResult()
            {
                Token = jwtToken,
                RefreshToken = refreshTokenInfo.RefreshToken,
                Success = true
            };
        }

        private string GetRandomString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        public async Task<AuthenticationResult> Refresh(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Now we need to check if the token has a valid security algorithm
            var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if (result == false)
                    return new AuthenticationResult()
                    {
                        Success = false,
                        Errors = new List<string>() { "Invalid security algorithm" }
                    };
            }

            // Will get the time stamp in unix time
            var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            // we convert the expiry date from seconds to the date
            var expDate = UnixTimeStampToDateTime(utcExpiryDate);
            if (expDate > DateTime.UtcNow)
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new List<string>() { "We cannot refresh this since the token has not expired" }
                };

            var storedRefreshToken = _context.RefreshTokenInfoTable.FirstOrDefault(x => x.RefreshToken == tokenRequest.RefreshToken);
            if (storedRefreshToken == null)
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new List<string>() { "Refresh token doesnt exist." }
                };

            // Check the date of the saved token if it has expired
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new List<string>() { "Token has expired, user needs to relogin" }
                };

            // we are getting here the jwt token id
            var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            // check the id that the recieved token has against the id saved in the db
            if (storedRefreshToken.JwtId != jti)
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new List<string>() { "The token doenst mateched the saved token" }
                };

            _context.RefreshTokenInfoTable.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(storedRefreshToken.UserId);
            return GenerateJWTToken(user);
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }
    }
}
