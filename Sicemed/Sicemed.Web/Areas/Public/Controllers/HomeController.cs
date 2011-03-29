using System.Web.Mvc;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web.Areas.Public.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Logger.Debug("ERRORRRRR!!!");
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