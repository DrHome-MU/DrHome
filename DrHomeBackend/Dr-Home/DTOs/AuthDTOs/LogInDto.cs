using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.AuthDTOs
{
    public class LogInDto
    {
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        public string Email { get; set; }

        [Required]
        [MinLength(10)]
        public string Password { get; set; }
    }
}
