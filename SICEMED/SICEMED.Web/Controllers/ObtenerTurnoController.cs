using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.ObtenerTurno;
using Sicemed.Web.Models.ViewModels.ObtenerTurno;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt]
    public class ObtenerTurnoController : BaseController
    {
        public virtual IObtenerEspecialidadesQuery ObtenerEspecialidadesQuery { get; set; }
        public virtual IObtenerProfesionalPorEspecialidadONombreQuery ObtenerProfesionalPorEspecialidadONombreQuery { get; set; }

        public virtual ActionResult Index()
        {
            var model = new BusquedaProfesionalViewModel();
            model.Especialidades = ObtenerEspecialidadesQuery.Execute();
            return View(model);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateModelState]
        [ValidateAntiForgeryToken]
        public ActionResult BuscarProfesional(BusquedaProfesionalViewModel model)
        {
            model.Especialidades = ObtenerEspecialidadesQuery.Execute();

            ObtenerProfesionalPorEspecialidadONombreQuery.EspecialidadId = model.EspecialidadId;
            ObtenerProfesionalPorEspecialidadONombreQuery.Profesional = model.Profesional;

            model.ProfesionalesEncontrados = ObtenerProfesionalPorEspecialidadONombreQuery.Execute();

            return View("Index", model);
        }
    }
}