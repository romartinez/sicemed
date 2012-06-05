using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class DomainController : NHibernateController
    {
        [HttpGet]        
        public ActionResult GetLocalidades(int provinciaId)
        {
            return Json(DomainExtensions.GetLocalidades(SessionFactory, provinciaId), JsonRequestBehavior.AllowGet);           
        }

        [HttpGet]
        public ActionResult NuevoTelefono()
        {
            return PartialView("_Telefono", new Telefono());
        }

        [HttpGet]
        public ActionResult GetPlanesObraSocial(long obraSocialId)
        {
            return Json(DomainExtensions.GetPlanesObraSocial(SessionFactory, obraSocialId), JsonRequestBehavior.AllowGet);
        }
    }
}
