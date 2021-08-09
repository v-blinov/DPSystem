using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ApiDPSystem.Models;
using ApiDPSystem.Records;
using ApiDPSystem.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiDPSystem.Services
{
    public class AccountService
    {
        public delegate Task RegisterHandler(User user, string subject, string message);


        private const string UserRole = "User";

        private readonly AccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;


        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserService userService,
            EmailService emailService,
            IConfiguration configuration,
            TokenValidationParameters tokenValidationParameters,
            AccountRepository accountRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;

            _accountRepository = accountRepository;
        }

        public event RegisterHandler SendMessage;


        public async Task<IdentityResult> LogInAsync(LogInRecord logInModel)
        {
            var checkResult = await IsEmailConfirmedAsync(logInModel.Email);
            if (!checkResult.Succeeded)
                return checkResult;

            var signInResult = await _signInManager.PasswordSignInAsync(logInModel.Email, logInModel.Password, false, false);
            if (!signInResult.Succeeded)
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Неправильный логин и(или) пароль."
                });

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRecord registerModel, string url)
        {
            var user = new User
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName
            };

            var createUserOperationResult = await _userManager.CreateAsync(user, registerModel.Password);
            if (!createUserOperationResult.Succeeded)
                return createUserOperationResult;

            var addRoleOperationResult = await _userService.AddRoleToUser(user, UserRole);
            if (!addRoleOperationResult.Succeeded)
            {
                await _userService.RemoveUser(user.Email);
                return addRoleOperationResult;
            }

            SendMessage += _emailService.SendEmailAsync;
            await SendRegistrationEmailAsync(user, url);

            return IdentityResult.Success;
        }

        private async Task SendRegistrationEmailAsync(User user, string url)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            url = url.Replace("userIdValue", user.Id);
            url = url.Replace("codeValue", HttpUtility.UrlEncode(code));

            await SendMessage?.Invoke(user, "Confirm your account", $"Подтвердите регистрацию, перейдя по ссылке: <a href='{url}'>Confirm your email</a>");
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string code)
        {
            if (userId == null || code == null)
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded;
        }

        public async Task ForgotPasswordAsync(EmailRecord emailRecord, string url)
        {
            var result = await IsEmailConfirmedAsync(emailRecord.Email);

            if (result.Succeeded)
            {
                SendMessage += _emailService.SendEmailAsync;

                var user = await _userManager.FindByEmailAsync(emailRecord.Email);
                await SendForgotPasswordEmailAsync(user, url);
            }
        }

        private async Task SendForgotPasswordEmailAsync(User user, string url)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            url = url.Replace("userIdValue", user.Id);
            url = url.Replace("codeValue", code);

            await SendMessage?.Invoke(user, "Reset password", $"Для сброса пароля пройдите по ссылке: <a href='{url}'>link</a>");
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordRecord resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);

            if (user == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Пользователь с email {resetPassword.Email} не найден."
                });

            return await _userManager.ResetPasswordAsync(user, resetPassword.Code, resetPassword.Password);
        }

        private async Task<IdentityResult> IsEmailConfirmedAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Пользователь с email {email} не найден."
                });

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Email {email} не подтвержден."
                });

            return IdentityResult.Success;
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AuthenticationResult> GenerateJWTTokenAsync(string userEmail)
        {
            var user = await FindUserByEmailAsync(userEmail);
            var role = await _userService.GetRole(user);

            return await GenerateJWTTokenAsync(user, role);
        }

        public async Task<AuthenticationResult> GenerateJWTTokenAsync(User user, string role)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("roles", role)
                }),
                Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = await _accountRepository.GetRefreshTokenAsync(token.Id, user);

            return new AuthenticationResult
            {
                Token = jwtToken,
                RefreshToken = refreshToken,
                Success = true
            };
        }

        public async Task<AuthenticationResult> RefreshAsync(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Now we need to check if the token has a valid security algorithm
            var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if (!result)
                    return new AuthenticationResult
                    {
                        Success = false,
                        Errors = new List<string> {"Invalid Token"}
                    };
            }

            var storedRefreshToken = _accountRepository.GetStoredRefreshToken(tokenRequest.RefreshToken);
            if (storedRefreshToken == null)
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new List<string> {"Refresh token doesnt exist."}
                };

            // Check the date of the saved token if it has expired
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new List<string> {"Token has expired, user needs to relogin"}
                };

            // we are getting here the jwt token id
            // check the id that the recieved token has against the id saved in the db
            var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            if (storedRefreshToken.JwtId != jti)
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new List<string> {"The token didn't matech the saved token"}
                };

            await _accountRepository.UpdateStoredRefreshTokenAsync(storedRefreshToken);

            var user = await _userManager.FindByIdAsync(storedRefreshToken.UserId);
            var role = await _userService.GetRole(user);

            return await GenerateJWTTokenAsync(user, role);
        }

        public async Task<IdentityResult> RegisterExternalUser(User user)
        {
            return await _userManager.CreateAsync(user);
        }
    }
}