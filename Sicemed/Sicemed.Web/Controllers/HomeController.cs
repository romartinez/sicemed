using System.Web.Mvc;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Logger.FatalFormat("ERRORRRRR!!!");

            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Register()
        {
            return RedirectToAction("About");
        }
    }
}