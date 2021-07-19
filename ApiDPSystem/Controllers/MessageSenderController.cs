using ApiDPSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiDPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageSenderController : ControllerBase
    {
        private readonly RabbitMqService _rabbitMqService;

        public MessageSenderController(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public IActionResult SendMessage([FromForm] string message)
        {
            _rabbitMqService.Publish(message);
            return Ok();
        }
    }
}
