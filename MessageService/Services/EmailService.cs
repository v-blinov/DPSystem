﻿using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MessageService.Models;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

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
            emailMessage.Subject = rabbitMessage.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = rabbitMessage.Message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465);
                await client.AuthenticateAsync(_configuration["EmailSender:AuthorEmail"], _configuration["EmailSender:AuthorPassword"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}