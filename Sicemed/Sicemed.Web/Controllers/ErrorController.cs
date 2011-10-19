using System.Web.Mvc;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;

namespace Sicemed.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult HttpError404(string message)
        {
            ViewData.Model = message;
            return View();
        }

        public ActionResult HttpError500(string message)
        {
            ViewData.Model = message;
            return View();
        }

        public ActionResult General(string message)
        {
            ViewData.Model = message;
            return View();
        }
    }
}