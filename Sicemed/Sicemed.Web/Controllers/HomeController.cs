using System.Web.Mvc;
using Castle.Core.Logging;

namespace Sicemed.Web.Controllers {
    public class HomeController : Controller {
        public ILogger Logger { get; set; }

        public ActionResult Index() {
            Logger.FatalFormat("ERRORRRRR!!!");

            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About() {
            return View();
        }
    }
}
