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
    public class NotificationsControllerTest
    {
        [TestMethod]
        public async Task NotifiationListAsync()
        {
            // Arrange
            NotificationsController controller = new NotificationsController();

            // Act
            ActionResult result = await controller.Index();
            var viewResult = (ViewResult)result;
            var objlist = (List<Notification>)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, objlist.Count);
        }

        [TestMethod]
        public async Task NotifiationDetailsAsync()
        {
            // Arrange
            NotificationsController controller = new NotificationsController();

            // Act
            ActionResult result = await controller.Details(1);
            var viewResult = (ViewResult)result;
            var objlist = (Notification)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Title 1", objlist.Title);
        }

        [TestMethod]
        public async Task NotifiationEditAsync()
        {
            // Arrange
            NotificationsController controller = new NotificationsController();
            NotificationView item = new NotificationView()
            {
                NotificationId = 1,
                Title = "Title 1",
                Body = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. ",
                IsAvailable = false
            };

            // Act
            ActionResult result = await controller.Edit(item);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
