using WeatherAPI.Application.DtoModels;
using WeatherAPI.Domain;

namespace WeatherAPI.Application.Mappings
{
    public static class CountryMapping
    {
        public static CountryDto ToCountryDto(this Country country)
        {
            return new CountryDto
            {
                Id = country.Id,
                Name = country.Name,
                Regions = country.Regions.Select(r => r.Name).ToList()
            };
        }
    }
}
