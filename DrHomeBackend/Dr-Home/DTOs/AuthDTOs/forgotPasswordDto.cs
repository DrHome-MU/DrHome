using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.AuthDTOs;

public class forgotPasswordDto
{
    [Required]
    [EmailAddress(ErrorMessage ="Email Address is not valid")]
    public string Email { get; set; }
}