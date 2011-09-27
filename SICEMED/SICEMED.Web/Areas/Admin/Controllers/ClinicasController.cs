using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Paginas;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    [AuthorizeIt(typeof(Administrador))]
    public class ClinicasController : BaseController
    {
        public virtual IObtenerClinicaActivaQuery ObtenerClinicaActivaQuery { get; set; }

        public virtual ActionResult Index()
        {
            var clinica = ObtenerClinicaActivaQuery.Execute();

            return View(clinica);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute]
        public virtual ActionResult Guardar(Clinica model)
        {
            var modelFromDb = ObtenerClinicaActivaQuery.Execute();

            UpdateModel(modelFromDb);

            ViewBag.Message = "Los datos de la clínica fueron actualizados exitosamente.";

            return View("Index", modelFromDb);
        }
    }
}