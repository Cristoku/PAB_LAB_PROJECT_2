namespace WeatherAPI.Domain
{
    public class Weather
    {
        public int Id { get; set; }

        public int Temperature { get; set; }

        public string? Feeling { get; set; }

        public Region Region { get; set; }
    }
}