namespace WeatherAPI.Application.DtoModels
{
    public class CountryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<string> Regions { get; set; }
    }
}
