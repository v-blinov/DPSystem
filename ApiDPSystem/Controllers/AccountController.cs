using ApiDPSystem.Extension;
using ApiDPSystem.Models;
using ApiDPSystem.Records;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RegisterService _registerService;
        private readonly EmailService _emailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RegisterService registerService, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _registerService = registerService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<ApiResponse> LogIn([FromForm] LogInRecord logInModel)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("В метод LogIn в контроллере AccountController отправлена невалидная модель.");

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
                var user = await _userManager.FindByNameAsync(logInModel.Email);
                if (user != null)
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                        return new ApiResponse()
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status403Forbidden,
                            Message = "Почта не подтверждена."
                        };

                var result = await _signInManager.PasswordSignInAsync(logInModel.Email, logInModel.Password, false, false);

                if (result.Succeeded)
                    return new ApiResponse()
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK
                    };

                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Неправильный логин и(или) пароль."
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
        public async Task<ApiResponse> Register([FromForm] RegisterRecord registerModel)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("предоставлены некорректные данные для метода RegisterModel");

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
                if (await _userManager.FindByNameAsync(registerModel.Email) != null)
                    return new ApiResponse()
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Message = "Пользователь с таким email уже существует."
                    };

                var user = new User
                {
                    UserName = registerModel.Email,
                    Email = registerModel.Email,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                };
                string url = Url.Action("ConfirmEmail", "Account", new { userId = "userIdValue", code = "codeValue" }, protocol: HttpContext.Request.Scheme);

                _registerService.SendMessage += _emailService.SendEmailAsync;
                var response = await _registerService.Register(user, registerModel.Password, url);

                if (response.Succeeded)
                    return new ApiResponse()
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK
                    };
                    
                return new ApiResponse()
                    {
                        IsSuccess = response.Succeeded,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Ошибка регистрации пользователя.",
                        Errors = response.Errors.Select(p => p.Description).ToList()
                    };
            }
            catch(Exception ex)
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
        public async Task<ApiResponse> ConfirmEmail(string userId, string code)
        {
            try
            {
                bool response = await _registerService.ConfirmEmail(userId, code);
                if (response)
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
    }
}
