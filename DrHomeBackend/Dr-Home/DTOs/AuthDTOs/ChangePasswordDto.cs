using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.AuthDTOs
{
    public class ChangePasswordDto
    {
        [MinLength(10)]
        public required string NewPassword { get; set; }
        [MinLength(10)]
        public required string ConfirmNewPassword { get; set; }
    }
}
