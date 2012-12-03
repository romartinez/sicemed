using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Listados;

namespace Sicemed.Web.Controllers
{
    public class ListadosController : BaseController
    {
        public ActionResult Profesionales()
        {
            var profesionales = QueryFactory.Create<IListadoProfesionalesQuery>().Execute();
            return View(profesionales);
        }

        public ActionResult ObraSociales()
        {
            var obrasociales = QueryFactory.Create<IListadoObraSocialesQuery>().Execute();
            return View(obrasociales);
        }
    }
}