using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Secretaria;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Secretaria))]
    public class SecretariaController : NHibernateController
    {
         public ActionResult Presentacion()
         {
             var viewModel = QueryFactory.Create<IObtenerTurnosPorFechaQuery>().Execute();
             return View(viewModel);
         }

        public ActionResult Otorgar()
         {             
             return View();
         }
    }
}