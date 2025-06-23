namespace Dr_Home.Data.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public string SenderName { get; set; } = string.Empty;

        public string SenderPhoneNumber {  get; set; } = string.Empty;


        public string SenderEmail { get; set; } = string.Empty;


        public string Content {  get; set; } = string.Empty;    


    }
}
