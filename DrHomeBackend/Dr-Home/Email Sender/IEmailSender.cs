using Dr_Home.DTOs.EmailSender;

namespace Dr_Home.Email_Sender
{
    public interface IEmailSender
    {
        Task SendRegisterEmailAsync(SendEmailRegisterDto dto);
    }
}
