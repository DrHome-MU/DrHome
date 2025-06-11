namespace Dr_Home.DTOs.AuthDTOs
{
    public class ActiveAccountResponse
    {
        public Guid UserId { get; set; }

        public required string Email { get; set; }

        public required string Token { get; set; }

    }
}
