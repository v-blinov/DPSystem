using ApiDPSystem.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ApiDPSystem.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private const string userRole = "User";

        public EmailService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task SendEmailAsync(User user, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_configuration["EmailSender:AuthorName"], _configuration["EmailSender:AuthorEmail"]));
            emailMessage.To.Add(new MailboxAddress("", user.Email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };


            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_configuration["EmailSender:AuthorEmail"], _configuration["EmailSender:AuthorPassword"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public async Task<List<string>> GetUserEmailsAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync(userRole);
            return users.Where(p => p.EmailConfirmed).Select(p => p.Email).ToList();
        }
    }
}
