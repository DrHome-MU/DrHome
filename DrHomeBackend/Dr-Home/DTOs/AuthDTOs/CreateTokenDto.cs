using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.AuthDTOs
{
    public class CreateTokenDto
    {
        public Guid? Id { get; set; }
        
        [MaxLength(100)]
        public string? FullName { get; set; }

        [MaxLength(20)]
        [RegularExpression("^(Admin|Doctor|Patient|أدمن|مريض|دكتور)$",
            ErrorMessage = "The type must be either Admin Or Patient Or Doctor.")]
        public string? role { get; set; }
        
        public string? Email { get; set; }
    }
}
