using WeatherAPI.Application.DtoModels;
using WeatherAPI.Domain;

namespace WeatherAPI.Application.Mappings
{
    public static class RegionMapping
    {
        public static RegionDto ToRegionDto(this Region region)
        {
            return new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Country = region.Country.Name
            };
        }
    }
}
