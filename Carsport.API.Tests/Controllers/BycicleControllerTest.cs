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
    public class BycicleControllerTest
    {
        [TestMethod]
        public void bycicleListAsync()
        {
            // Arrange
            ByciclesController controller = new ByciclesController();

            // Act
            IQueryable<Bycicle> result = controller.GetBycicles();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.ToList().Count);
            Assert.IsInstanceOf<Bycicle>(result.ToList()[0]);
        }

        [TestMethod]
        public async Task GetBycicleAsync()
        {
            // Arrange
            ByciclesController controller = new ByciclesController();

            // Act
            IHttpActionResult result = await controller.GetBycicle(1);
            var route = result as OkNegotiatedContentResult<Bycicle>;

            // Assert
            Assert.IsNotNull(route);
            Assert.AreEqual(1, route.Content.BycicleId);
        }
    }
}
