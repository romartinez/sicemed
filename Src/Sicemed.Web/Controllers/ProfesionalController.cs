using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Profesional;
using Sicemed.Web.Models;
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
    }
}