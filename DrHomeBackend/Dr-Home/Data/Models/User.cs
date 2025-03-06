using System.ComponentModel.DataAnnotations;

namespace Dr_Home.Data.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [MaxLength(100)]
        [Required]
         public string FullName { get; set; }

        [Required]
        [MaxLength(10)]
        [RegularExpression("^(Male|Female|male|female|ذكر|أنثى)$",
            ErrorMessage = "The Gender must be male or female")]
        public string Gender { get; set; }
        
        [EmailAddress(ErrorMessage ="Email Address is not valid")]
        public string Email { get; set; }
        
        [MaxLength(11)]
        [MinLength(11)]
        public string? PhoneNumber { get; set; }

        public string HashPassword { get; set; }
        
        [MaxLength(20)]
        [RegularExpression("^(Admin|Doctor|Patient|أدمن|مريض|دكتور)$", 
            ErrorMessage = "The type must be either Admin Or Patient Or Doctor.")]
        public string role { get; set; }

        public string? city { get; set; }

        public string? region { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpired { get; set; }
        
        public string? ConfirmationCode { get; set; }

        public bool IsActive { get; set; }
    }
}
