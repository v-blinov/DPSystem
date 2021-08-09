using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class TestController : Controller
    {
        [HttpGet]
        [Authorize(Policy = "User")]
        public IActionResult CheckAuthentication()
        {
            return Ok("Success access");
        }
    }
}