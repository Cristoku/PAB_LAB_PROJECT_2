using WeatherAPI.Application.DtoModels;
using WeatherAPI.Domain;

namespace WeatherAPI.Application.Mappings
{
    public static class WeatherMapping
    {
        public static WeatherDto ToWeatherDto(this Weather weather)
        {
            return new WeatherDto
            {
                Id = weather.Id,
                Temperature = weather.Temperature,
                Feeling = weather.Feeling ?? "",
                Region = weather.Region.Name,
                Country = weather.Region.Country.Name
            };
        }
    }
}
