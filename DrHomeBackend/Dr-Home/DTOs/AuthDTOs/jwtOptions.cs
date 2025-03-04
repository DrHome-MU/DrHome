
namespace Dr_Home.DTOs.AuthDTOs
{
    public class jwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationTime { get; set; }
        public string Key { get; set; }
    }
}
