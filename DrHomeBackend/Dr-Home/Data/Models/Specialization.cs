using System.ComponentModel.DataAnnotations;

namespace Dr_Home.Data.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? PicturePath { get; set; }
        public List<Doctor >? Doctors { get; set; }
    }
}
