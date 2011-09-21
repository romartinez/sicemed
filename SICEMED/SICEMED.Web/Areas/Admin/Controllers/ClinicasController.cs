using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    [AuthorizeIt(typeof(Administrador))]
    public class ClinicasController : NHibernateController
    {
        public virtual ActionResult Index()
        {
            //Obtengo la última clínica
            var session = SessionFactory.GetCurrentSession();

            var clinica = session.QueryOver<Clinica>().List().LastOrDefault();

            return View(clinica);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute]
        public virtual ActionResult Guardar(Clinica model)
        {
            var session = SessionFactory.GetCurrentSession();

            var modelFromDb = session.QueryOver<Clinica>().Where(x => x.Id == model.Id).SingleOrDefault();

            UpdateModel(modelFromDb);

            ViewBag.Message = "Los datos de la clínica fueron actualizados exitosamente.";

            return View("Index", modelFromDb);
        }
    }
}