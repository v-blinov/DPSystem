using ApiDPSystem.Extension;
using ApiDPSystem.Models;
using ApiDPSystem.Records;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        private readonly EmailService _emailService;

        public AccountController(AccountService registerService, EmailService emailService)
        {
            _accountService = registerService;
            _emailService = emailService;
        }


        [HttpPost]
        public async Task<ApiResponse<AuthenticationResult>> LogIn([FromForm] LogInRecord logInModel)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("В метод LogIn в контроллере AccountController отправлена невалидная модель.");
                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Введены некорректные логин и(или) пароль.",
                    Errors = ModelState.GetErrorList()
                };
            }

            try
            {
                var logInResult = await _accountService.LogIn(logInModel);
                if (!logInResult.Succeeded)
                    return new ApiResponse<AuthenticationResult>()
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Ошибка при входе в аккаунт",
                        Errors = logInResult.Errors.Select(p => p.Description).ToList()
                    };

                var tokenResult = await _accountService.GenerateJWTTokenAsync(logInModel.Email);
                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Content = tokenResult,
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }

        [HttpPost]
        public async Task<ApiResponse> RegisterUser([FromForm] RegisterRecord registerModel)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Предоставлены некорректные данные для метода RegisterModel");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Введены некорректные данные.",
                    Errors = ModelState.GetErrorList()
                };
            }

            try
            {
                string confirmEmailUrl = Url.Action("ConfirmEmail", "Account", new { userId = "userIdValue", code = "codeValue" }, protocol: HttpContext.Request.Scheme);

                var registerResult = await _accountService.Register(registerModel, confirmEmailUrl);
                if (registerResult.Succeeded)
                    return new ApiResponse()
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK
                    };

                return new ApiResponse()
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Ошибка регистрации пользователя.",
                        Errors = registerResult.Errors.Select(p => p.Description).ToList()
                    };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }

        [HttpGet]
        public async Task<ApiResponse> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            try
            {
                bool result = await _accountService.ConfirmEmail(userId, code);
                if (result)
                    return new ApiResponse()
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK
                    };

                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка при подтверждении email.",
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }

        [HttpPost]
        public async Task<ApiResponse> ForgotPassword([FromForm] ForgotPasswordRecord forgotPassword)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("предоставлены некорректные данные для метода ForgotPassword");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Введен некорректный email адрес.",
                    Errors = ModelState.GetErrorList()
                };
            }

            try
            {
                var result = await _accountService.CheckIfEmailConfirmedAsync(forgotPassword.Email);
                if (result.Succeeded)
                {
                    string url = Url.Action("ResetPassword", "Account", new { userId = "userIdValue", code = "codeValue" }, protocol: HttpContext.Request.Scheme);

                    _accountService.SendMessage += _emailService.SendEmailAsync;
                    await _accountService.ForgotPassword(forgotPassword.Email, url);
                }

                return new ApiResponse()
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка в методе ForgotPassword."
                };
            }
        }

        [HttpPost]
        public async Task<ApiResponse> ResetPassword([FromForm] ResetPasswordRecord resetPassword)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Предоставлены некорректные данные для метода RegisterModel");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Введены некорректные логин и(или) пароль.",
                    Errors = ModelState.GetErrorList()
                };
            }

            try
            {
                var result = await _accountService.ResetPassword(resetPassword);

                if (result.Succeeded)
                    return new ApiResponse()
                    {
                        IsSuccess = result.Succeeded,
                        StatusCode = StatusCodes.Status200OK
                    };

                return new ApiResponse()
                {
                    IsSuccess = result.Succeeded,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка при изменении пароля.",
                    Errors = result.Errors.Select(p => p.Description).ToList()
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка при изменении пароля."
                };
            }
        }

        [HttpPost]
        public async Task<ApiResponse<AuthenticationResult>> RefreshToken([FromForm] TokenRequest tokenRequest)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Предоставлены некорректные данные для метода RefreshToken");
                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Поля AccessToken и RefreshToken обязательны.",
                    Errors = ModelState.GetErrorList()
                };
            }

            try
            {
                var refreshTokenResult = await _accountService.Refresh(tokenRequest);

                if (refreshTokenResult.Success)
                    return new ApiResponse<AuthenticationResult>()
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Content = refreshTokenResult
                    };

                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid tokens.",
                    Errors = refreshTokenResult.Errors
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка при обновлении AccessToken."
                };
            }
        }

        [HttpPost]
        public IActionResult DecodeJwtToken([FromForm] string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            //var jti = tokenS.Claims.First(claim => claim.Type == "jti").Value;
            return Ok(jsonToken);
        }
    }
}
