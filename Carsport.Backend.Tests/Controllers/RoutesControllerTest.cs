using Carsport.Backend.Controllers;
using Carsport.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Carsport.Backend.Tests.Controllers
{
    [TestClass]
    public class RoutesControllerTest
    {
        [TestMethod]
        public async Task RoutesListAsync()
        {
            // Arrange
            RoutesController controller = new RoutesController();

            // Act
            ActionResult result = await controller.Index();
            var viewResult = (ViewResult)result;
            var objlist = (List<Route>)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, objlist.Count);
        }

        [TestMethod]
        public async Task RouteDetailsAsync()
        {
            // Arrange
            RoutesController controller = new RoutesController();

            // Act
            ActionResult result = await controller.Details(21);
            var viewResult = (ViewResult)result;
            var objlist = (Route)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(21, objlist.RouteId);
        }


        [TestMethod]
        public async Task RouteDeleteAsync()
        {
            // Arrange
            RoutesController controller = new RoutesController();

            // Act
            ActionResult result = await controller.DeleteConfirmed(21);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
