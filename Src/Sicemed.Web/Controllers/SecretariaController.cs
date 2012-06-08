using System.Web.Mvc;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Secretaria;
using Sicemed.Web.Models;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarIngresoPaciente(long turnoId)
        {
            var session = SessionFactory.GetCurrentSession();
            var turno = session.Get<Turno>(turnoId);
            if (turno == null || turno.SePresento)
            {
                ShowMessages(ResponseMessage.Error("No se encuentra el turno o ya se encuentra otorgado."));
                return RedirectToAction("Presentacion");
            }

            turno.RegistrarIngreso(User.As<Secretaria>());

            ShowMessages(ResponseMessage.Success());
            return RedirectToAction("Presentacion");
        }
    }
}