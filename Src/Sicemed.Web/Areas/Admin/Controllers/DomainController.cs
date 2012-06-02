using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class DomainController : NHibernateController
    {
        [HttpGet]        
        public ActionResult GetLocalidades(int provinciaId)
        {
            return Json(DomainExtensions.GetLocalidades(SessionFactory, provinciaId), JsonRequestBehavior.AllowGet);           
        }
    }
}
