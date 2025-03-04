namespace Dr_Home.DTOs.EmailSender
{
    public class SendEmailRegisterDto
    {
       public  required string subject {  get; set; }    
       public   required string toEmail { get; set;  }
       public required string  message { get; set; }
    }
}
