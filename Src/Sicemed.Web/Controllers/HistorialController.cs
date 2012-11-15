using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Queries.Historial;
using Sicemed.Web.Infrastructure.Reports;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;
using Sicemed.Web.Models.ViewModels.Historial;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Profesional))]
    [AuthorizeIt(typeof(Paciente))]
    [AuthorizeIt(typeof(Secretaria))]
    public class HistorialController : NHibernateController
    {

        [HttpGet]
        [AuthorizeIt(typeof(Profesional))]
        [AuthorizeIt(typeof(Secretaria))]
        public ActionResult SeleccionPaciente()
        {
            return View(new SeleccionPacienteViewModel());
        }

        [HttpPost]
        [ValidateModelState]
        [AuthorizeIt(typeof(Profesional))]
        [AuthorizeIt(typeof(Secretaria))]
        public ActionResult SeleccionPaciente(SeleccionPacienteViewModel viewModel)
        {
            return RedirectToAction("TurnosPorPaciente", new { viewModel.PacienteId });
        }

        public ActionResult TurnosPorPaciente(TurnosPorPacienteViewModel viewModel)
        {
            //Si es sólo paciente, puede ver sus atenciones únicamente
            if (User.IsInRole<Paciente>() && User.Roles.Count == 1) viewModel.PacienteId = User.As<Paciente>().Id;

            if (!viewModel.PacienteId.HasValue) return RedirectToAction("SeleccionPaciente");

            var paciente = SessionFactory.GetCurrentSession().Get<Paciente>(viewModel.PacienteId);
            if (paciente == null) return HttpNotFound();


            viewModel.PacienteSeleccionado = MappingEngine.Map<InfoViewModel>(paciente);

            var query = QueryFactory.Create<IObtenerTurnosPorPacienteQuery>();
            query.FechaDesde = viewModel.Filters.Desde;
            query.FechaHasta = viewModel.Filters.Hasta;
            query.Filtro = viewModel.Filters.Filtro;
            query.PacienteId = paciente.Id;

            viewModel.Turnos = query.Execute();

            return View(viewModel);
        }

        public ActionResult FichaPacienteReporte(TurnosPorPacienteViewModel viewModel)
        {
            //Si es sólo paciente, puede ver sus atenciones únicamente
            if (User.IsInRole<Paciente>() && User.Roles.Count == 1) viewModel.PacienteId = User.As<Paciente>().Id;

            if (!viewModel.PacienteId.HasValue) return RedirectToAction("SeleccionPaciente");

            var paciente = SessionFactory.GetCurrentSession().Get<Paciente>(viewModel.PacienteId);
            if (paciente == null) return HttpNotFound();

            viewModel.PacienteSeleccionado = MappingEngine.Map<InfoViewModel>(paciente);

            var report = ReportFactory.Create<IFichaPacienteReporte>();
            report.FechaDesde = viewModel.Filters.Desde;
            report.FechaHasta = viewModel.Filters.Hasta;
            report.Filtro = viewModel.Filters.Filtro;
            report.PacienteId = paciente.Id;

            var reportEngine = new ReportEngine();
            var reportBytes = reportEngine.BuildReport(report);
            return File(reportBytes, "application/pdf", string.Format("FichaPaciente_{0}.pdf", viewModel.PacienteSeleccionado.Descripcion.SanitanizeForFileName()));
        }
    }
}