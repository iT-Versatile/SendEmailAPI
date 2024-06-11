using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace SendEmailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("mailtrap@demomailtrap.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(
                _configuration["SmtpSettings:Server"],
                int.Parse(_configuration["SmtpSettings:Port"]),
                MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(
                _configuration["SmtpSettings:User"],
                _configuration["SmtpSettings:Password"]);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
