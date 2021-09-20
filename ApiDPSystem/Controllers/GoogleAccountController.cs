using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApiDPSystem.Models;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class GoogleAccountController : Controller
    {
        private const string UserRole = "User";
        private readonly AccountService _accountService;
        private readonly string _googleRedirectUrlEndpoint;
        private readonly SignInManager<User> _signInManager;
        private readonly UserService _userService;

        public GoogleAccountController(IConfiguration configuration, AccountService accountService, SignInManager<User> signInManager, UserService userService)
        {
            _accountService = accountService;
            _signInManager = signInManager;
            _userService = userService;
            _googleRedirectUrlEndpoint = configuration.GetValue<string>("OAuth:GoogleRedirectUrl");
        }

        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var provider = "Google";

            var redirectUrl = _googleRedirectUrlEndpoint;
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<ApiResponse<AuthenticationResult>> GoogleResponse()
        {
            try
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                    return new ApiResponse<AuthenticationResult>
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Данные пользователя не обнаружены."
                    };

                //возможно стоит редиректнуть на страницу входа

                //Если внешний пользователь уже сохранен в нашей базе, то генерируем токены
                var user = await _userService.GetUserByEmail(info.Principal.FindFirst(ClaimTypes.Email)?.Value);
                if (user != null)
                {
                    var authenticationResult = await _accountService.GenerateJwtTokenAsync(user, UserRole);
                    return new ApiResponse<AuthenticationResult>
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Content = authenticationResult
                    };
                }

                //Если внешний пользователь не сохранен в нашей базе,
                //то сначала сохраняем его, затем генерируем токены

                user = new User
                {
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.Name).Split(" ")[0],
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Name).Split(" ")[1],
                    EmailConfirmed = true
                };

                var externalRegisterResult = await _accountService.RegisterExternalUser(user);

                if (externalRegisterResult.Succeeded)
                {
                    var roleAddingResult = await _userService.AddRoleToUser(user, UserRole);
                    if (roleAddingResult.Succeeded)
                    {
                        var authenticationResult = await _accountService.GenerateJwtTokenAsync(user, UserRole);

                        return new ApiResponse<AuthenticationResult>
                        {
                            IsSuccess = true,
                            StatusCode = StatusCodes.Status200OK,
                            Content = authenticationResult
                        };
                    }

                    Log.Error($"Ошибка при добавлении роли {UserRole} для пользователя {user.Email}.");
                    await _userService.RemoveUser(user.Email);

                    return new ApiResponse<AuthenticationResult>
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Ошибка регистрации пользователя.",
                        Errors = roleAddingResult.Errors.Select(p => p.Description).ToList()
                    };
                }

                Log.Error($"Ошибка при регистрации пользователя {user.Email}. {externalRegisterResult.Errors.Select(p => p.Description).ToList()}");
                return new ApiResponse<AuthenticationResult>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка регистрации пользователя.",
                    Errors = externalRegisterResult.Errors.Select(p => p.Description).ToList()
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }
    }
}