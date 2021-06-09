using ApiDPSystem.Models;
using ApiDPSystem.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;


        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> LogIn([FromForm] LogInRecord logInModel)
        {
            try
            {
                var response = await signInManager.PasswordSignInAsync(logInModel.Email, logInModel.Password, false, false);

                if (response.Succeeded)
                {
                    var user = userManager.Users.SingleOrDefault(u => u.Email == logInModel.Email);
                    return Ok(GenerateJwtToken(user));
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] SignUpRecord signUpModel)
        {
            try
            {
                var user = new User
                {
                    FirstName = signUpModel.FirstName,
                    LastName = signUpModel.LastName,
                    Email = signUpModel.Email,
                    UserName = signUpModel.Email
                };
                
                var response = await userManager.CreateAsync(user, signUpModel.Password);

                if (response.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return Ok(GenerateJwtToken(user));
                }
                else
                {
                    Log.Error($"Метод SignUp в api контроллере AccountController: userManager.CreateAsync вернул response.Secceeded = false. \n{response.Errors}");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[] {
                new Claim("Name", $"{user.FirstName} {user.LastName}"),
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiresTime = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpireMinutes"]));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiresTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
