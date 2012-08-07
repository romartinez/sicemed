using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Busqueda;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Controllers
{
    public class BusquedaController : BaseController
    {
        public ActionResult Paciente(string filtro)
        {
            var query = QueryFactory.Create<IBusquedaPersonaQuery>();
            query.Rol = typeof (Paciente);
            query.Filtro = filtro;
            return Json(query.Execute(), JsonRequestBehavior.AllowGet);
        }

    }
}
