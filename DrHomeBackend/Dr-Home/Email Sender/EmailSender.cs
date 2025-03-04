using Dr_Home.DTOs.EmailSender;
using System.Net;
using System.Net.Http;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Dr_Home.Email_Sender
{
    public class EmailSender(IConfiguration _configuration) : IEmailSender
    {
        public async Task SendRegisterEmailAsync(SendEmailRegisterDto dto)
        {
            Console.WriteLine("A7a");
            var email = new MimeMessage();

            var sender = _configuration["EmailData:Email"];
            var appPassword = _configuration["EmailData:Password"];
            var host = _configuration["EmailData:Host"];
            var port = _configuration["EmailData:Port"];

            email.From.Add(MailboxAddress.Parse(sender));
            email.To.Add(MailboxAddress.Parse(dto.toEmail));
            email.Subject = dto.subject;
            email.Body = new TextPart(TextFormat.Html) { Text = dto.message };

            using var smtp = new SmtpClient();

            smtp.Connect(host, Convert.ToInt32(port), SecureSocketOptions.StartTls);
            smtp.Authenticate(sender, appPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
