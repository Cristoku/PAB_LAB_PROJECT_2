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
    public class RegionController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllData()
        {
            var result = context.Regions
                .Include(x => x.Country)
                .Select(r => r.ToRegionDto())
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetDataById(int id)
        {
            var region = context.Regions
                .Include(x => x.Country)
                .FirstOrDefault(x => x.Id == id);

            if (region == null)
                return NotFound();

            return Ok(region.ToRegionDto());
        }

        [HttpPost]
        public IActionResult AddData(string regionName, int countryId)
        {
            Region region = new Region()
            {
                Name = regionName,
                Country = context.Countries.Find(countryId)
            };
            context.Regions.Add(region);
            context.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateData(int resgionId, string regionName, int countryId)
        {
            Region region = context.Regions.Find(resgionId);
            region.Name = regionName;
            region.Country = context.Countries.Find(countryId);
            context.Regions.Update(region);
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteData(int id)
        {
            var region = context.Regions.Find(id);

            if (region == null)
                return NotFound();

            context.Regions.Remove(region);
            context.SaveChanges();
            return Ok();
        }
    }
}
