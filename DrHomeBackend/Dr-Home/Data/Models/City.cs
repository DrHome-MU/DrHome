namespace Dr_Home.Data.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Region>? _regions { get; set; }
    }
}
