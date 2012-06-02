using System.Web.Mvc;
using AutoMapper;
using Sicemed.Web.Areas.Admin.Models.Clinicas;
using Sicemed.Web.Infrastructure;
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

        	AppendLists(viewModel);

            return View(viewModel);
        }

        private void AppendLists(ClinicaEditViewModel viewModel)
        {
            viewModel.TiposDocumentosHabilitados = DomainExtensions.GetTiposDocumentos(viewModel.DocumentoTipoDocumentoValue);
            viewModel.ProvinciasHabilitadas = DomainExtensions.GetProvincias(SessionFactory, viewModel.DomicilioLocalidadProvinciaId);

            if (viewModel.DomicilioLocalidadProvinciaId.HasValue)
            {
                viewModel.LocalidadesHabilitadas = 
                    DomainExtensions.GetLocalidades(SessionFactory, viewModel.DomicilioLocalidadProvinciaId.Value, viewModel.DomicilioLocalidadId);
            }
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Guardar(ClinicaEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                AppendLists(viewModel);
                return View("Index", viewModel);
            }

            var modelFromDb = ObtenerClinicaActivaQuery.Execute();

            var viewModelFromDb = Mapper.Map<ClinicaEditViewModel>(modelFromDb);

            UpdateModel(viewModelFromDb);

            Mapper.Map(viewModelFromDb, modelFromDb);

            ShowMessages(ResponseMessage.Success());

            return RedirectToAction("Index");
        }
    }
}