using System;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Profesional;
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
    }
}