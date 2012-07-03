using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;

namespace Sicemed.Web.Controllers
{
    public class PingController : BaseController
    {
         public ActionResult Index()
         {
             Logger.DebugFormat("Pinging...");
             return null;
         }
    }
}