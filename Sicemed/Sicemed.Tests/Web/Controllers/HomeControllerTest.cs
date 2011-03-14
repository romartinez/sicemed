using Castle.Core.Logging;
using Moq;
using NUnit.Framework;
using Sicemed.Web.Controllers;

namespace Sicemed.Tests.Web.Controllers {
    [TestFixture]
    public class HomeControllerTest {
        [Test]
        public void Index() {
            // Arrange
            HomeController controller = new HomeController();
            var logger = new Mock<ILogger>();
            controller.Logger = logger.Object;

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.That("Welcome to ASP.NET MVC!", Is.EqualTo(result.ViewBag.Message));
        }

        [Test]
        public void About() {
            // Arrange
            HomeController controller = new HomeController();
            var logger = new Mock<ILogger>();
            controller.Logger = logger.Object;

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}
