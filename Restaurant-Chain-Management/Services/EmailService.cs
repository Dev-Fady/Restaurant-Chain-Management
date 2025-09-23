using System.Net;
using System.Net.Mail;

namespace Restaurant_Chain_Management.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;

        public EmailService(IConfiguration config)
        {
            this.config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(config["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(config["EmailSettings:Port"]),
                Credentials = new NetworkCredential(config["EmailSettings:Username"], config["EmailSettings:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(config["EmailSettings:SenderEmail"], config["EmailSettings:SenderName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
