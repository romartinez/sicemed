using System.Web.Mvc;
using Sicemed.Web.Models;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Logger.Debug("ERRORRRRR!!!");
            using(var session = SessionFactory.GetCurrentSession())
            using(var tx = session.BeginTransaction())
            {
                var cal = session.QueryOver<Calendario>().Where(x => x.Id == 1).SingleOrDefault();
                ViewBag.Message = cal.Nombre;

                tx.Commit();
            }

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