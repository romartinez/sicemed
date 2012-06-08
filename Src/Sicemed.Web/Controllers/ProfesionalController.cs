using System;
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
    public class ProfesionalController : NHibernateController
    {
         public ActionResult Agenda(DateTime? fecha = null)
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
            if (turno == null || turno.SeAtendio)
            {
                ShowMessages(ResponseMessage.Error("No se encuentra el turno o ya se encuentra atendido."));
                return RedirectToAction("Agenda");
            }

            turno.RegistrarAtencion(nota);

            ShowMessages(ResponseMessage.Success());
            return RedirectToAction("Agenda");            
        }
    }
}