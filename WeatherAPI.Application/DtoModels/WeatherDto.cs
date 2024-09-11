namespace WeatherAPI.Application.DtoModels
{
    public class WeatherDto
    {
        public int Id { get; set; }

        public int Temperature { get; set; }

        public string Feeling { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }
    }
}
