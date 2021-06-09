using ApiDPSystem.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [HttpPost]
        public IActionResult LogIn([FromForm] LogInRecord logInModel)
        {
            //try
            //{
            //    var


            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex, "");
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}
            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        //public IActionResult SignUp([FromForm] SignUpRecord signUpModel)
        //{ 

        //}

        //public IActionResult LogOut()
        //{ }




    }
}
