using System.Web.Mvc;
using Castle.Core.Logging;
using Moq;
using NUnit.Framework;
using Sicemed.Web.Areas.Public.Controllers;

namespace Sicemed.Tests.Web.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void About()
        {
            // Arrange
            var controller = new HomeController();
            var logger = new Mock<ILogger>();
            controller.Logger = logger.Object;

            // Act
            var result = controller.About() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Index()
        {
            // Arrange
            var controller = new HomeController();
            var logger = new Mock<ILogger>();
            controller.Logger = logger.Object;

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.That("Welcome to ASP.NET MVC!", Is.EqualTo(result.ViewBag.Message));
        }
    }
}