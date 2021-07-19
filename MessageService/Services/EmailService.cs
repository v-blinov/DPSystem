using MailKit.Net.Smtp;
using MailKit.Security;
using MessageService.Models;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(RabbitMessage rabbitMessage)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_configuration["EmailSender:AuthorName"], _configuration["EmailSender:AuthorEmail"]));
            emailMessage.To.AddRange(rabbitMessage.Addresses.Select(p => new MailboxAddress("", p)));
            //emailMessage.To.Add(new MailboxAddress("", address));
            emailMessage.Subject = rabbitMessage.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = rabbitMessage.Message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_configuration["EmailSender:AuthorEmail"], _configuration["EmailSender:AuthorPassword"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
