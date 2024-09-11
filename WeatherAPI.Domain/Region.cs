namespace WeatherAPI.Domain
{
    public class Region
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Country Country { get; set; }
    }
}
