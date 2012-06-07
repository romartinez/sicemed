using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Domain;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class DomainController : NHibernateController
    {
        [HttpGet]        
        public ActionResult GetEspecialidades()
        {
            var query = QueryFactory.Create<IObtenerEspecialidadesDropDownQuery>();
            var results = query.Execute();
            return Json(results, JsonRequestBehavior.AllowGet);           
        }

        [HttpGet]        
        public ActionResult GetLocalidades(long provinciaId)
        {
            var query = QueryFactory.Create<IObtenerLocalidadesPorProvinciaDropDownQuery>();
            query.ProvinciaId = provinciaId;
            var results = query.Execute();
            return Json(results, JsonRequestBehavior.AllowGet);           
        }

        [HttpGet]
        public ActionResult GetPlanesObraSocial(long obraSocialId)
        {
            var query = QueryFactory.Create<IObtenerPlanesPorObraSocialDropDownQuery>();
            query.ObraSocialId = obraSocialId;
            var results = query.Execute();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult NuevoTelefono()
        {
            return PartialView("_Telefono", new Telefono());
        }
    }
}
