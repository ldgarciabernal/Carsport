using Carsport.API.Controllers;
using Carsport.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Assert = NUnit.Framework.Assert;

namespace Carsport.API.Tests.Controllers
{
    [TestClass]
    public class RoutesControllerTest
    {
        [TestMethod]
        public void RouteListAsync()
        {
            // Arrange
            RoutesController controller = new RoutesController();

            // Act
            IQueryable<Route> result = controller.GetRoutes();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.ToList().Count);
            Assert.IsInstanceOf<Route>(result.ToList()[0]);
        }

        [TestMethod]
        public async Task GetRouteAsync()
        {
            // Arrange
            RoutesController controller = new RoutesController();

            // Act
            IHttpActionResult result = await controller.GetRoute(23);
            var route = result as OkNegotiatedContentResult<Route>;

            // Assert
            Assert.IsNotNull(route);
            Assert.AreEqual(23, route.Content.RouteId);
        }
    }
}
