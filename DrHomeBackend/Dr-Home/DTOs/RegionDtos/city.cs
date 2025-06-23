using System.Text.Json.Serialization;

namespace Dr_Home.DTOs.RegionDtos
{
    public class Governorate
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("governorate_name_ar")]
        public string Governorate_Name_Ar { get; set; } = string.Empty;

        [JsonPropertyName("governorate_name_en")]
        public string Governorate_Name_En { get; set; } = string.Empty;
    }
}
