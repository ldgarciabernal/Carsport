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
    public class MailTemplatesControllerTest
    {
        [TestMethod]
        public async Task MailTemplateListAsync()
        {
            // Arrange
            MailTemplatesController controller = new MailTemplatesController();

            // Act
            ActionResult result = await controller.Index();
            var viewResult = (ViewResult)result;
            var objlist = (List<MailTemplate>)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, objlist.Count);
        }

        [TestMethod]
        public async Task MailTemplateDetailsAsync()
        {
            // Arrange
            MailTemplatesController controller = new MailTemplatesController();

            // Act
            ActionResult result = await controller.Details(1);
            var viewResult = (ViewResult)result;
            var objlist = (MailTemplate)viewResult.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, objlist.MailTemplateId);
        }
    }
}
