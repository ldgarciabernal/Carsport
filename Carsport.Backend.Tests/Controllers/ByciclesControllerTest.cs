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
    public class ByciclesControllerTest
    {
        [TestMethod]
        public async Task BycicleListAsync()
        {
            // Arrange
            ByciclesController controller = new ByciclesController();

            // Act
            ActionResult result = await controller.Index();
            var viewResult = (ViewResult)result;
            var objlist = (List<Bycicle>)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, objlist.Count);
        }

        [TestMethod]
        public async Task BycicleDetailsAsync()
        {
            // Arrange
            ByciclesController controller = new ByciclesController();

            // Act
            ActionResult result = await controller.Details(1);
            var viewResult = (ViewResult)result;
            var objlist = (Bycicle)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, objlist.BycicleId);
        }

        [TestMethod]
        public async Task BycicleCreateAsync()
        {
            // Arrange
            ByciclesController controller = new ByciclesController();
            BycicleView item = new BycicleView()
            {
                BycicleId = 999,
                Street = "Title 1",
                University = "univerity 1",
                Description = "description 1",
                Latitude = "232",
                Longitude = "223",
                IsAvailable = false
            };

            // Act
            ActionResult result = await controller.Create(item);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task BycicleEditAsync()
        {
            // Arrange
            ByciclesController controller = new ByciclesController();
            BycicleView item = new BycicleView()
            {
                BycicleId = 999,
                Street = "Title 1",
                University = "univerity 1",
                Description = "description 1",
                Latitude = "232",
                Longitude = "223",
                IsAvailable = false
            };

            // Act
            ActionResult result = await controller.Edit(item);

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
