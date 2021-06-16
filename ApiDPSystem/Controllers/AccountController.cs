using ApiDPSystem.Models;
using ApiDPSystem.Records;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
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
        public async Task<IActionResult> LogIn([FromForm] LogInRecord logInModel)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("В метод LogIn в контроллере AccountController отправлена невалидная модель.");
                return BadRequest(logInModel);
            }

            try
            {
                var user = await _userManager.FindByNameAsync(logInModel.Email);
                if (user != null)
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                        return StatusCode(StatusCodes.Status403Forbidden, "Почта не подтвержденаю");

                var result = await _signInManager.PasswordSignInAsync(logInModel.Email, logInModel.Password, logInModel.RememberMe, false);
                
                if (result.Succeeded)
                    return Ok();
                
                return Unauthorized("Неправильный логин и(или) пароль");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterRecord registerModel)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("RegisterModel предоставлены некорректные данные для метода RegisterModel");
                return BadRequest("Введены некорректные логин и (или) пароль.");
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

                _registerService.SendMessage += _emailService.SendEmailAsync;
                var response = await _registerService.Register(user, registerModel.Password, url);

                if (response)
                    return Ok();

                return BadRequest();
            }
            catch(Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                bool response = await _registerService.ConfirmEmail(userId, code);
                if (response)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
