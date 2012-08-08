using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Busqueda;

namespace Sicemed.Web.Controllers
{
    public class BusquedaController : BaseController
    {
        public ActionResult Paciente(string filtro)
        {
            var query = QueryFactory.Create<IBusquedaGeneralPacienteQuery>();
            query.Filtro = filtro;
            return Json(query.Execute(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Profesional(string filtro)
        {
            var query = QueryFactory.Create<IBusquedaProfesionalConEspecialidadesQuery>();
            query.Filtro = filtro;
            return Json(query.Execute(), JsonRequestBehavior.AllowGet);
        }

    }
}
