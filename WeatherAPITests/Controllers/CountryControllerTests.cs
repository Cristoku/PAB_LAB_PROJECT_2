using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Application.DtoModels;
using WeatherAPI.Domain;
using WeatherAPI.Persistence;

namespace WeatherAPI.Controllers.Tests
{
    [TestClass()]
    public class CountryControllerTests
    {
        static CountryController GetCountryController()
        {
            Random random = new();
            string randomDbName = random.Next().ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: randomDbName);
            var testDb = new ApplicationDbContext(optionsBuilder.Options);

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

            testDb.Countries.Add(country);
            testDb.Regions.AddRange(regions);
            testDb.SaveChanges();

            Weather weather = new Weather()
            {
                Temperature = -20,
                Feeling = "Mroźno",
                Region = regions[0]
            };
            testDb.Weathers.Add(weather);
            testDb.SaveChanges();

            return new CountryController(testDb);
        }

        [TestMethod()]
        public void GetAllDataTest()
        {
            var controller = GetCountryController();

            var result = controller.GetAllData();
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var countryDtos = okResult.Value as List<CountryDto>;
            Assert.IsNotNull(countryDtos);
            Assert.AreEqual(1, countryDtos.Count);
        }

        [TestMethod()]
        public void GetDataByIdOkTest()
        {
            var controller = GetCountryController();

            var result = controller.GetDataById(1);
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var countryDto = okResult.Value as CountryDto;
            Assert.IsNotNull(countryDto);
            Assert.AreEqual(1, countryDto.Id);
            Assert.AreEqual("Polska", countryDto.Name);
        }

        [TestMethod()]
        public void GetDataByIdNokTest()
        {
            var controller = GetCountryController();

            var result = controller.GetDataById(2) as NotFoundResult;
            Assert.IsNotNull(result);

            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod()]
        public void AddDataTest()
        {
            var controller = GetCountryController();

            var result = controller.AddData("Niemcy") as OkResult;
            Assert.IsNotNull(result);

            var checkResult = controller.GetDataById(2) as OkObjectResult;
            Assert.IsNotNull(result);
            var newEntity = checkResult.Value as CountryDto;
            Assert.AreEqual("Niemcy", newEntity.Name);
        }

        [TestMethod()]
        public void UpdateDataTest()
        {
            var controller = GetCountryController();

            var result = controller.UpdateData(1, "Niemcy") as OkResult;
            Assert.IsNotNull(result);

            var checkResult = controller.GetDataById(1) as OkObjectResult;
            var countryDto = checkResult.Value as CountryDto;
            Assert.IsNotNull(countryDto);
            Assert.AreEqual(1, countryDto.Id);
            Assert.AreEqual("Niemcy", countryDto.Name);
        }

        [TestMethod()]
        public void DeleteDataTest()
        {
            var controller = GetCountryController();

            var result = controller.DeleteData(1) as OkResult;
            Assert.IsNotNull(result);

            var checkResult = controller.GetDataById(1) as NotFoundResult;
            Assert.IsNotNull(checkResult);

            Assert.AreEqual(404, checkResult.StatusCode);
        }
    }
}