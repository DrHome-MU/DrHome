using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.AuthDTOs
{
    public class RegisterDto
    {
        [MaxLength(100)]
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }
        
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        public string Email { get; set; }
        
        [MaxLength(11)]
        [MinLength(11)]
        [RegularExpression(@"^01[0125][0-9]{8}$",
            ErrorMessage = "رقم الهاتف غير صحيح، يجب أن يكون 11 رقم ويبدأ بـ 01")]
        public string? PhoneNumber { get; set; }
        
        [Required]
        [MinLength(10)]
        public string Password { get; set; }

        [Compare("Password",
            ErrorMessage = "Make sure that the confirmed password is equal to the password")]
        public string ConfirmPassword { get; set; }
        
        public DateOnly? DateOfBirth { get; set; }
    }
}
