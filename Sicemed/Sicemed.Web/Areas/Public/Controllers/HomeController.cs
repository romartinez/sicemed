using System.Web.Mvc;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web.Areas.Public.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Inicio()
        {
            Logger.Debug("ERRORRRRR!!!");
            return View();
        }

        public ActionResult Acerca()
        {
            return View();
        }
    }
}