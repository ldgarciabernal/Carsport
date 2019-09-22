using Carsport.Backend.Controllers;
using Carsport.Backend.Models;
using Carsport.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Carsport.Backend.Tests.Controllers
{
    [TestClass]
    public class CitiesControllerTest
    {
        [TestMethod]
        public async Task CitiesListAsync()
        {
            // Arrange
            CitiesController controller = new CitiesController();

            // Act
            ActionResult result = await controller.Index();
            var viewResult = (ViewResult)result;
            var objlist = (List<City>)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, objlist.Count);
        }

        [TestMethod]
        public async Task CityDetailsAsync()
        {
            // Arrange
            CitiesController controller = new CitiesController();

            // Act
            ActionResult result = await controller.Details(1);
            var viewResult = (ViewResult)result;
            var objlist = (City)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, objlist.CityId);
        }

        [TestMethod]
        public async Task CityEditAsync()
        {
            // Arrange
            CitiesController controller = new CitiesController();
            City item = new City()
            {
                CityId = 999,
                Name = "ciudad editada"
            };

            // Act
            ActionResult result = await controller.Edit(item);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CityCreateAsync()
        {
            // Arrange
            CitiesController controller = new CitiesController();
            City item = new City()
            {
                CityId = 999,
                Name = "ciudad"
            };

            // Act
            ActionResult result = await controller.Create(item);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task BycicleDeleteAsync()
        {
            // Arrange
            ByciclesController controller = new ByciclesController();

            // Act
            ActionResult result = await controller.DeleteConfirmed(999);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
