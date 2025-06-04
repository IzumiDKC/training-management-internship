using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

namespace training_management_internship.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();

            // Người gửi
            emailMessage.From.Add(new MailboxAddress("Training System", _config["EmailSettings:From"]));

            // Người nhận
            emailMessage.To.Add(MailboxAddress.Parse(email));

            // Tiêu đề và nội dung
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            // Gửi email qua SMTP
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    _config["EmailSettings:Host"],
                    int.Parse(_config["EmailSettings:Port"]),
                    SecureSocketOptions.StartTls
                );

                await client.AuthenticateAsync(
                    _config["EmailSettings:Sender"],
                    _config["EmailSettings:Password"]
                );

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
