using Carsport.Backend.Controllers;
using Carsport.Backend.Models;
using Carsport.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Carsport.Backend.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        [TestMethod]
        public void UserListAsync()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act
            ActionResult result = controller.Index();
            var viewResult = (ViewResult)result;
            var objlist = (List<UserView>)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, objlist.Count);
        }

        [TestMethod]
        public async Task UserDetailsAsync()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act
            ActionResult result = await controller.Details("663438ec-0c79-489d-84ee-10e9b241378b");
            var viewResult = (ViewResult)result;
            var objlist = (UserView)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("663438ec-0c79-489d-84ee-10e9b241378b", objlist.UserId);
        }


        [TestMethod]
        public async Task UserDeleteAsync()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act b03d071f-076e-4a89-a027-f4fa21312533 before create user with this id
            ActionResult result = await controller.DeleteConfirmed("b03d071f-076e-4a89-a027-f4fa21312533");

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
