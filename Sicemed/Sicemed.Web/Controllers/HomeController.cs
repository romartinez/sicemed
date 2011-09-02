using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;

namespace SICEMED.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public virtual ActionResult About()
        {
            return View();
        }
    }
}
