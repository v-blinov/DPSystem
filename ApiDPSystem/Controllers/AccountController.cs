﻿using ApiDPSystem.Extension;
using ApiDPSystem.Models;
using ApiDPSystem.Records;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        private readonly EmailService _emailService;
        private readonly string _clientId;
        private readonly string _clientSecret;

        private readonly string _accountUrlEndpoint;
        private readonly string _redirectUrlEndpoint;
        private readonly string _revokeUrlEndpoint;
        private readonly string _tokenUrlEndpoint;

        public AccountController(AccountService registerService,
                                 EmailService emailService,
                                 IConfiguration configuration)
        {
            _accountService = registerService;
            _emailService = emailService;
            _clientId = configuration.GetValue<string>("Authentication:Google:ClientId");
            _clientSecret = configuration.GetValue<string>("Authentication:Google:ClientSecret");

            _accountUrlEndpoint = configuration.GetValue<string>("OAuth:AccountUrlEndpoint");
            _tokenUrlEndpoint = configuration.GetValue<string>("OAuth:TokenUrlEndpoint");
            _revokeUrlEndpoint = configuration.GetValue<string>("OAuth:RevokeUrlEndpoint");
            _redirectUrlEndpoint = configuration.GetValue<string>("OAuth:RedirectUrlEndpoint");
        }

        [HttpGet]
        public IActionResult OAuthGoogleLoginGetUrl()
        {
            try
            {
                string url = $"{_accountUrlEndpoint}?" +
                             $"client_id={_clientId}&" +
                             $"redirect_uri={_redirectUrlEndpoint}&" +
                              "response_type=code&" +
                              "access_type=offline&" +
                              "prompt=consent&" +
                              "scope=openid%20profile%20email";

                return Ok(url);
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> OAuthGetAccessToken(string code)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new Dictionary<string, string>()
                    {
                        { "code", code },
                        { "client_id", _clientId },
                        { "client_secret", _clientSecret },
                        { "redirect_uri", HttpUtility.UrlDecode(_redirectUrlEndpoint) },
                        { "access_type", "offline" },
                        { "prompt", "consent" },
                        { "grant_type", "authorization_code" }
                    };

                    var formContent = new FormUrlEncodedContent(data);
                    var apiResponse = await client.PostAsync(_tokenUrlEndpoint, formContent);
                    var result = await apiResponse.Content.ReadAsStringAsync();

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> OAuthRefreshTokenAsync(string refreshToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new Dictionary<string, string>()
                    {
                        { "client_id", _clientId },
                        { "client_secret", _clientSecret },
                        { "refresh_token", refreshToken },
                        { "grant_type", "refresh_token" },
                    };

                    var formContent = new FormUrlEncodedContent(data);
                    var apiResponse = await client.PostAsync(_tokenUrlEndpoint, formContent);
                    var result = await apiResponse.Content.ReadAsStringAsync();

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> OAuthRevokeTokenAsync(string accessToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new Dictionary<string, string>()
                    {
                        { "client_id", _clientId },
                        { "client_secret", _clientSecret },
                        { "token", accessToken }
                    };

                    var formContent = new FormUrlEncodedContent(data);
                    await client.PostAsync(_revokeUrlEndpoint, formContent);
                    
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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
                var result = await _accountService.LogIn(logInModel);
                if (result.Succeeded)
                {
                    var user = await _accountService.FindUserByEmail(logInModel.Email);
                    var authenticationResult = _accountService.GenerateJWTToken(user);

                    if (authenticationResult.Success)
                        return new ApiResponse<AuthenticationResult>()
                        {
                            IsSuccess = true,
                            StatusCode = StatusCodes.Status200OK,
                            Content = authenticationResult
                        };
                    else
                        return new ApiResponse<AuthenticationResult>()
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Ошибка при попытке аутентификации пользователя.",
                            Errors = authenticationResult.Errors
                        };
                }

                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка при входе в аккаунт",
                    Errors = result.Errors.Select(p => p.Description).ToList()
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
        public async Task<ApiResponse> Register([FromForm] RegisterRecord registerModel)
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
                var user = new User
                {
                    UserName = registerModel.Email,
                    Email = registerModel.Email,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                };
                string url = Url.Action("ConfirmEmail", "Account", new { userId = "userIdValue", code = "codeValue" }, protocol: HttpContext.Request.Scheme);

                _accountService.SendMessage += _emailService.SendEmailAsync;
                var result = await _accountService.Register(user, registerModel.Password, url);

                if (result.Succeeded)
                    return new ApiResponse()
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK
                    };

                return new ApiResponse()
                {
                    IsSuccess = result.Succeeded,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ошибка регистрации пользователя.",
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

        [Authorize]
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
                    Message = "Введены некорректные логин и(или) пароль.",
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

        [Authorize]
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
                var verifyTokenResult = await _accountService.VerifyToken(tokenRequest);

                if (verifyTokenResult == null)
                    return new ApiResponse<AuthenticationResult>()
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid tokens.",
                    };

                return new ApiResponse<AuthenticationResult>()
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Content = verifyTokenResult
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

        #region AdditionalFunctionality
        //[HttpGet]
        //public async Task<IActionResult> GoogleResponse()
        //{
        //    try
        //    {
        //        ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //            return RedirectToAction("Login");

        //        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        //        string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
        //        if (result.Succeeded)
        //            return Ok(userInfo);
        //        else
        //        {
        //            User user = new User
        //            {
        //                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
        //                UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
        //                FirstName = "TEST",
        //                LastName = "test"
        //                //LastName = info.Principal.FindFirstValue(ClaimTypes.Name).Split(" ")[1] 
        //            };

        //            IdentityResult identResult = await _userManager.CreateAsync(user);
        //            if (identResult.Succeeded)
        //            {
        //                identResult = await _userManager.AddLoginAsync(user, info);
        //                if (identResult.Succeeded)
        //                {
        //                    await _signInManager.SignInAsync(user, false);
        //                    return Ok(userInfo.ToList());
        //                }
        //            }
        //            return StatusCode(StatusCodes.Status403Forbidden);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("", ex);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
        #endregion
    }
}
