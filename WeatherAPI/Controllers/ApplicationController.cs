using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Application;
using WeatherAPI.Domain;
using WeatherAPI.Persistence;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class ApplicationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUserService userService) : ControllerBase
    {
        [HttpGet]
        public IActionResult Login(string username, string password)
        {
            return Ok(userService.Login(username, password));
        }

        [HttpPost]
        public IActionResult Seed()
        {
            Country country = new Country()
            {
                Name = "Polska"
            };

            List<Region> regions = new List<Region>
            {
                new Region()
                {
                    Name = "Mazowieckie",
                    Country = country
                },
                new Region()
                {
                    Name = "Śląskie",
                    Country = country
                }
            };

            context.Countries.Add(country);
            context.Regions.AddRange(regions);
            context.SaveChanges();

            Weather weather = new Weather()
            {
                Temperature = -20,
                Feeling = "Mroźno",
                Region = regions[0]
            };
            context.Weathers.Add(weather);
            context.SaveChanges();

            IdentityRole adminRole = new("Admin");
            IdentityRole userRole = new("User");
            roleManager.CreateAsync(adminRole).Wait();
            roleManager.CreateAsync(userRole).Wait();

            IdentityUser admin = new IdentityUser()
            {
                UserName = "Admin"
            };
            userManager.CreateAsync(admin, "Adm1n3k!").Wait();
            userManager.AddToRoleAsync(admin, "Admin").Wait();

            IdentityUser user = new IdentityUser()
            {
                UserName = "User"
            };
            userManager.CreateAsync(user, "US3r3k!").Wait();
            userManager.AddToRoleAsync(user, "User").Wait();

            return Ok("Przygotowano przykładowe dane");
        }
    }
}
