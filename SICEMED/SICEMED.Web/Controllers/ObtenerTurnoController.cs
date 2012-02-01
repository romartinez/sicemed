using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt]
    public class ObtenerTurnoController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}