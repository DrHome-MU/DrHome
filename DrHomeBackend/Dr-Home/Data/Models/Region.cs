namespace Dr_Home.Data.Models
{
    public class Region
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }

        public City _city { get; set; }
    }
}
