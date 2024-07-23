using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;


namespace DocumentManagement.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(SendEmailDTOs SendEmailDTOs)
        {        
            var emailSettings = _configuration.GetSection("EmailSettings");

            string smtpServer = emailSettings["SmtpServer"];
            int smtpPort = int.Parse(emailSettings["SmtpPort"]);
            string smtpUsername = emailSettings["SmtpUsername"];
            string smtpPassword = emailSettings["SmtpPassword"];
            string fromEmail = emailSettings["FromEmail"];

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.To.Add(SendEmailDTOs.ToEmail);
                mailMessage.Subject = SendEmailDTOs.Subject;
                mailMessage.Body = SendEmailDTOs.Body;
                mailMessage.IsBodyHtml = true;

                await client.SendMailAsync(mailMessage);
            }
            
        }    
    }
}
