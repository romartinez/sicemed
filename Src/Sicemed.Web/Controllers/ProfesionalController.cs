using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Profesional;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Profesional))]
    public class ProfesionalController : NHibernateController
    {
         public ActionResult Agenda()
         {
             var query = QueryFactory.Create<IObtenerAgendaProfesionalQuery>();
             query.ProfesionalId = User.As<Profesional>().Id;
             var viewModel = query.Execute();
             return View(viewModel);
         }
    }
}