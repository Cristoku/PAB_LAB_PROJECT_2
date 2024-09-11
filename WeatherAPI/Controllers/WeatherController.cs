using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Application.Mappings;
using WeatherAPI.Domain;
using WeatherAPI.GrpcServiceClasses;
using WeatherAPI.Persistence;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin,User")]
    public class WeatherController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllData()
        {
            var result = context.Weathers
                .Include(x => x.Region)
                .ThenInclude(r => r.Country)
                .Select(w => w.ToWeatherDto())
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetDataById(int id)
        {
            var weather = context.Weathers
                .Include(x => x.Region)
                .ThenInclude(r => r.Country)
                .SingleOrDefault(x => x.Id == id);

            if (weather == null)
                return NotFound();

            return Ok(weather.ToWeatherDto());
        }

        [HttpPost]
        public IActionResult AddData(int temperature, int regionId)
        {
            Weather weather = new()
            {
                Temperature = temperature,
                Region = context.Regions.Find(regionId)
            };

            weather.NameWeather();
            context.Weathers.Add(weather);
            context.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateData(int weatherId, int temperature, int regionId)
        {
            Weather weather = context.Weathers.Find(weatherId);
            weather.Temperature = temperature;
            weather.Region = context.Regions.Find(regionId);
            weather.NameWeather();
            context.Weathers.Update(weather);
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteData(int id)
        {
            var weather = context.Weathers.Find(id);

            if (weather == null)
                return NotFound();

            context.Weathers.Remove(weather);
            context.SaveChanges();
            return Ok();
        }
    }
}