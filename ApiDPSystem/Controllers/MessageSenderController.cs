using System;
using System.Threading.Tasks;
using ApiDPSystem.Models;
using ApiDPSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace ApiDPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageSenderController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly RabbitMqService _rabbitMqService;

        public MessageSenderController(RabbitMqService rabbitMqService, EmailService emailService)
        {
            _rabbitMqService = rabbitMqService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageAsync([FromForm] string subject, [FromForm] string message)
        {
            try
            {
                var rabbitMessage = new RabbitMessage
                {
                    Addresses = await _emailService.GetUserEmailsAsync(),
                    Message = message,
                    Subject = subject
                };

                var jsonMessage = JsonConvert.SerializeObject(rabbitMessage);

                _rabbitMqService.Publish(jsonMessage);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}