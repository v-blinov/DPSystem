using System;
using System.Linq;
using System.Threading.Tasks;
using ApiDPSystem.Extensions;
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
        public async Task<ApiResponse<AuthenticationResult>> LogInAsync([FromForm] LogInRecord logInModel)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("В метод LogIn в контроллере AccountController отправлена невалидная модель.");
                return new ApiResponse<AuthenticationResult>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Введены некорректные логин и(или) пароль.",
                    Errors = ModelState.GetErrorList()
                };
            }

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
        public async Task<ApiResponse> RegisterUserAsync([FromForm] RegisterRecord registerModel)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Предоставлены некорректные данные для метода RegisterModel");
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Введены некорректные данные.",
                    Errors = ModelState.GetErrorList()
                };
            }

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
        public async Task<ApiResponse> ForgotPasswordAsync([FromForm] EmailRecord emailRecord)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("предоставлены некорректные данные для метода ForgotPassword");
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Введен некорректный email адрес.",
                    Errors = ModelState.GetErrorList()
                };
            }

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

        [HttpPost]
        [ActionName("ResetPassword")]
        public async Task<ApiResponse> ResetPasswordAsync([FromForm] ResetPasswordRecord resetPassword)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Предоставлены некорректные данные для метода RegisterModel");
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Введены некорректные логин и(или) пароль.",
                    Errors = ModelState.GetErrorList()
                };
            }

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
        public async Task<ApiResponse<AuthenticationResult>> RefreshTokenAsync([FromForm] TokenRequest tokenRequest)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Предоставлены некорректные данные для метода RefreshToken");
                return new ApiResponse<AuthenticationResult>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Поля AccessToken и RefreshToken обязательны.",
                    Errors = ModelState.GetErrorList()
                };
            }

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