using System.Web.Mvc;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web.Areas.Public.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ActionResult Inicio()
        {
            Logger.Debug("ERRORRRRR!!!");
            return View();
        }

        public virtual ActionResult Acerca()
        {
            return View();
        }
    }
}