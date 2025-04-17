namespace Dr_Home.Authentication
{
    public class jwtOptions
    {
        public static string SectionName { get; set; } = "Jwt";
        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty ;
        public int DurationTime { get; set; }

        [Required] 
        public string Key { get; set; } = string.Empty;
    }
}
