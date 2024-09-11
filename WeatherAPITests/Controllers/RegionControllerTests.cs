using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Application.DtoModels;
using WeatherAPI.Domain;
using WeatherAPI.Persistence;

namespace WeatherAPI.Controllers.Tests
{
    [TestClass()]
    public class RegionControllerTests
    {
        static RegionController GetRegionController()
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

            return new RegionController(testDb);
        }

        [TestMethod()]
        public void GetAllDataTest()
        {
            var controller = GetRegionController();

            var result = controller.GetAllData();
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var regionDtos = okResult.Value as List<RegionDto>;
            Assert.IsNotNull(regionDtos);
            Assert.AreEqual(2, regionDtos.Count);
        }

        [TestMethod()]
        public void GetDataByIdOkTest()
        {
            var controller = GetRegionController();

            var result = controller.GetDataById(1);
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var regionDto = okResult.Value as RegionDto;
            Assert.IsNotNull(regionDto);
            Assert.AreEqual(1, regionDto.Id);
            Assert.AreEqual("Mazowieckie", regionDto.Name);
        }

        [TestMethod()]
        public void GetDataByIdNokTest()
        {
            var controller = GetRegionController();

            var result = controller.GetDataById(3) as NotFoundResult;
            Assert.IsNotNull(result);

            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod()]
        public void AddDataTest()
        {
            var controller = GetRegionController();

            var result = controller.AddData("Lubuskie", 1) as OkResult;
            Assert.IsNotNull(result);

            var checkResult = controller.GetDataById(3) as OkObjectResult;
            Assert.IsNotNull(result);
            var newEntity = checkResult.Value as RegionDto;
            Assert.AreEqual("Lubuskie", newEntity.Name);
        }

        [TestMethod()]
        public void UpdateDataTest()
        {
            var controller = GetRegionController();

            var result = controller.UpdateData(1, "Lubuskie", 1) as OkResult;
            Assert.IsNotNull(result);

            var checkResult = controller.GetDataById(1) as OkObjectResult;
            var regionDto = checkResult.Value as RegionDto;
            Assert.IsNotNull(regionDto);
            Assert.AreEqual(1, regionDto.Id);
            Assert.AreEqual("Lubuskie", regionDto.Name);
        }

        [TestMethod()]
        public void DeleteDataTest()
        {
            var controller = GetRegionController();

            var result = controller.DeleteData(1) as OkResult;
            Assert.IsNotNull(result);

            var checkResult = controller.GetDataById(1) as NotFoundResult;
            Assert.IsNotNull(checkResult);

            Assert.AreEqual(404, checkResult.StatusCode);
        }
    }
}