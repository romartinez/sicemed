using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SICEMED.Web;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Profesional;
using Sicemed.Web.Infrastructure.Reports;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Reports;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Profesional))]
    public class ProfesionalController : AdministracionDeTurnosBaseController
    {
        public override ActionResult Agenda(DateTime? fecha = null)
        {
            var query = QueryFactory.Create<IObtenerAgendaProfesionalQuery>();
            query.ProfesionalId = User.As<Profesional>().Id;
            query.Fecha = fecha;
            var viewModel = query.Execute();
            return View(viewModel);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarAtencionPaciente(long turnoId, string nota = null)
        {
            var session = SessionFactory.GetCurrentSession();
            var turno = session.Get<Turno>(turnoId);
            if (turno == null)
            {
                ShowMessages(ResponseMessage.Error("No se encuentra el turno."));
                return RedirectToAction("Agenda");
            }

            turno.RegistrarAtencion(User.As<Profesional>(), nota);

            ShowMessages(ResponseMessage.Success());
            return RedirectToAction("Agenda", new { fecha = turno.FechaTurno.ToShortDateString() });
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarNota(long turnoId, string nota)
        {
            if(string.IsNullOrWhiteSpace(nota)) throw new ValidationException("Debe ingresa una nota.");

            var session = SessionFactory.GetCurrentSession();
            var turno = session.Get<Turno>(turnoId);
            if (turno == null)
            {
                ShowMessages(ResponseMessage.Error("No se encuentra el turno."));
                return RedirectToAction("Agenda");
            }

            turno.RegistrarNota(User.As<Profesional>(), nota);

            ShowMessages(ResponseMessage.Success());
            return RedirectToAction("Agenda", new { fecha = turno.FechaTurno.ToShortDateString() });
        }

        //public ActionResult Report()
        //{
        //    var report = new ServiceReport();
        //    var reportData = MvcApplication.Container.Resolve<ITurnosPorProfesionalReporte>();
        //    var reportInfo = new ReportInfo("TurnosPorProfesional", "Turnos", "", reportData.Execute);
        //    var reportBytes = report.BuildReport(reportInfo, "PDF");
        //    return File(reportBytes, "application/pdf", "reporte.pdf");
        //}
    }
}