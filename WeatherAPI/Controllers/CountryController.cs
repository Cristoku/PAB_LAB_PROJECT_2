using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Application.Mappings;
using WeatherAPI.Domain;
using WeatherAPI.Persistence;

namespace WeatherAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class CountryController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllData()
        {
            var result = context.Countries
                .Include(x => x.Regions)
                .Select(c => c.ToCountryDto())
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetDataById(int id)
        {
            var country = context.Countries
                .Include(x => x.Regions)
                .FirstOrDefault(x => x.Id == id);

            if (country == null)
                return NotFound();

            return Ok(country.ToCountryDto());
        }

        [HttpPost]
        public IActionResult AddData(string countryName)
        {
            Country country = new Country()
            {
                Name = countryName
            };
            context.Countries.Add(country);
            context.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateData(int countryId, string countryName)
        {
            Country country = context.Countries.Find(countryId);
            country.Name = countryName;
            context.Countries.Update(country);
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteData(int id)
        {
            var country = context.Countries.Find(id);

            if (country == null)
                return NotFound();

            context.Countries.Remove(country);
            context.SaveChanges();
            return Ok();
        }
    }
}
