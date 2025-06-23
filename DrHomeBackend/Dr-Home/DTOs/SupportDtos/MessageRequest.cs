namespace Dr_Home.DTOs.SupportDtos
{
    public record MessageRequest
    (
      string SenderName ,

      string SenderPhoneNumber,
      [EmailAddress(ErrorMessage = "Enter Correct Email Address")] 
      string SenderEmail,

      string Content 
  );
}
