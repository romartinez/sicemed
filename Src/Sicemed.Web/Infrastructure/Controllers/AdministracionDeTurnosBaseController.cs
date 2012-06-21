using System;
using System.Web.Mvc;
using Mvc.Mailer;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.Controllers
{
    [AuthorizeIt(typeof(Secretaria))]
    [AuthorizeIt(typeof(Paciente))]
    [AuthorizeIt(typeof(Profesional))]
    public abstract class AdministracionDeTurnosBaseController : NHibernateController
    {
        public abstract ActionResult Agenda(DateTime? fecha = null);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelarTurno(long turnoId, string prompt)
        {
            if(String.IsNullOrWhiteSpace(prompt))
            {
                ShowMessages(ResponseMessage.Error("Debe ingresar un motivo de cancelación."));
                return RedirectToAction("Agenda");
            }
            var session = SessionFactory.GetCurrentSession();
            var turno = session.Get<Turno>(turnoId);
            if (turno == null || !turno.PuedeAplicar(Turno.EventoTurno.Cancelar))
            {
                ShowMessages(ResponseMessage.Error("No se encuentra el turno o no se puede cancelar el mismo."));
                return RedirectToAction("Agenda");
            }

            turno.CancelarTurno(User, prompt);
            var mail = NotificationService.CancelacionTurno(User, turno);
            if (mail != null) mail.Send();

            ShowMessages(ResponseMessage.Success());
            return RedirectToAction("Agenda");
        }
    }
}