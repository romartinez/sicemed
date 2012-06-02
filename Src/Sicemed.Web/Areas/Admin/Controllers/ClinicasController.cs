using System.Web.Mvc;
using AutoMapper;
using Sicemed.Web.Areas.Admin.Models.Clinicas;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Queries.Paginas;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    [AuthorizeIt(typeof(Administrador))]
    public class ClinicasController : NHibernateController
    {
        public virtual IObtenerClinicaActivaQuery ObtenerClinicaActivaQuery { get; set; }

        public virtual ActionResult Index()
        {
            var model = ObtenerClinicaActivaQuery.Execute();
        	var viewModel = Mapper.Map<ClinicaEditViewModel>(model);

        	viewModel.TiposDocumentosHabilitados = DomainExtensions.GetTiposDocumentos(viewModel.DocumentoTipoDocumentoValue);
            viewModel.ProvinciasHabilitadas = DomainExtensions.GetProvincias(SessionFactory);

            return View(viewModel);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute]
        public virtual ActionResult Guardar(ClinicaEditViewModel viewModel)
        {
            var modelFromDb = ObtenerClinicaActivaQuery.Execute();

            var viewModelFromDb = Mapper.Map<ClinicaEditViewModel>(modelFromDb);

            UpdateModel(viewModelFromDb);

            Mapper.Map(viewModelFromDb, modelFromDb);

            ViewBag.Message = "Los datos de la clínica fueron actualizados exitosamente.";

            return View("Index", viewModelFromDb);
        }
    }
}