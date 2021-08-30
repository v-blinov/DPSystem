using System;
using System.Linq;
using System.Threading.Tasks;
using ApiDPSystem.Filters;
using ApiDPSystem.Models;
using ApiDPSystem.Records;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService registerService)
        {
            _accountService = registerService;
        }


        [HttpPost]
        [ActionName("LogIn")]
        [ModelValidationFilter("Введены некорректные данные.")]
        public async Task<ApiResponse<AuthenticationResult>> LogInAsync([FromForm] LogInRecord logInModel)
        {
            try
            {
                var logInResult = await _accountService.LogInAsync(logInModel);
                if (!logInResult.Succeeded)
                    return new ApiResponse<AuthenticationResult>
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Ошибка при входе в аккаунт",
                        Errors = logInResult.Errors.Select(p => p.Description).ToList()
                    };

                var tokenResult = await _accountService.GenerateJwtTokenAsync(logInModel.Email);
                return new ApiResponse<AuthenticationResult>
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Content = tokenResult
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

        [HttpPost]
        [ActionName("RegisterUser")]
        [ModelValidationFilter("Введены некорректные данные.")]
        public async Task<ApiResponse> RegisterUserAsync([FromForm] RegisterRecord registerModel)
        {
            try
            {
                var confirmEmailUrl = Url.Action("ConfirmEmail", "Account", new { userId = "userIdValue", code = "codeValue" }, HttpContext.Request.Scheme);

                var registerResult = await _accountService.RegisterAsync(registerModel, confirmEmailUrl);
                if (registerResult.Succeeded)
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK
                    };

                Log.Error("Тестовое логирование");

                return new ApiResponse
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
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }

        [HttpGet]
        [ActionName("ConfirmEmail")]
        public async Task<ApiResponse> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            try
            {
                var result = await _accountService.ConfirmEmailAsync(userId, code);
                if (result)
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK
                    };

                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка при подтверждении email."
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка на стороне сервера"
                };
            }
        }

        [HttpPost]
        [ActionName("ForgotPasswor")]
        [ModelValidationFilter("Введен некорректный email адрес.")]
        public async Task<ApiResponse> ForgotPasswordAsync([FromForm] EmailRecord emailRecord)
        {
            try
            {
                var url = Url.Action("ResetPassword", "Account", new { userId = "userIdValue", code = "codeValue" }, HttpContext.Request.Scheme);
                await _accountService.ForgotPasswordAsync(emailRecord, url);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка в методе ForgotPassword."
                };
            }
        }

        [HttpPatch]
        [ActionName("ResetPassword")]
        [ModelValidationFilter("Введены некорректные логин и(или) пароль.")]
        public async Task<ApiResponse> ResetPasswordAsync([FromForm] ResetPasswordRecord resetPassword)
        {
            try
            {
                var result = await _accountService.ResetPasswordAsync(resetPassword);

                if (result.Succeeded)
                    return new ApiResponse
                    {
                        IsSuccess = result.Succeeded,
                        StatusCode = StatusCodes.Status200OK
                    };

                return new ApiResponse
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
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка при изменении пароля."
                };
            }
        }

        [HttpPost]
        [ActionName("RefreshToken")]
        [ModelValidationFilter("Поля AccessToken и RefreshToken обязательны.")]
        public async Task<ApiResponse<AuthenticationResult>> RefreshTokenAsync([FromForm] TokenRequest tokenRequest)
        {
            try
            {
                var refreshTokenResult = await _accountService.RefreshAsync(tokenRequest);

                if (refreshTokenResult.Success)
                    return new ApiResponse<AuthenticationResult>
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Content = refreshTokenResult
                    };

                return new ApiResponse<AuthenticationResult>
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
                return new ApiResponse<AuthenticationResult>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Ошибка при обновлении AccessToken."
                };
            }
        }
    }
}