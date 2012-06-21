using System;
using System.Web.Mvc;
using AutoMapper;
using Mvc.Mailer;
using SICEMED.Web;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Enums;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Queries.ObtenerTurno;
using Sicemed.Web.Infrastructure.Queries.Secretaria;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels.Secretaria;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Secretaria))]
    public class SecretariaController : AdministracionDeTurnosBaseController
    {
        private readonly IMembershipService _membershipService;
        private readonly IMappingEngine _mappingEngine;

        public SecretariaController(IMappingEngine mappingEngine, IMembershipService membershipService)
        {
            _mappingEngine = mappingEngine;
            _membershipService = membershipService;
        }

        public override ActionResult Agenda(DateTime? fecha = null)
        {
            var query = QueryFactory.Create<IObtenerTurnosPorFechaQuery>();
            query.Fecha = fecha;
            var viewModel = query.Execute();
            return View(viewModel);
        }

        #region Otorgar Turno

        public ActionResult OtorgarTurno()
        {
            var editModel = new OtorgarTurnoEditModel();
            AppendLists(editModel);
            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OtorgarTurno(OtorgarTurnoEditModel editModel)
        {
            AppendLists(editModel);
            if(ModelState.IsValid)
            {
                var session = SessionFactory.GetCurrentSession();
                var paciente = session.Get<Paciente>(editModel.PacienteId.Value);
                var profesional = session.Get<Profesional>(editModel.ProfesionalId.Value);
                
                var especialidadId =
                    QueryFactory.Create<IObtenerEspecialidadesProfesionalDropDownQuery>()
                        .GetEspecialidadId(editModel.EspecialidadId);

                var especialidad = session.Get<Especialidad>(especialidadId);

                var queryTurno = QueryFactory.Create<IObtenerTurnosDisponiblesPorProfesionalDropDownQuery>();
                var consultorioId = queryTurno.GetConsultorioId(editModel.TurnoId);
                var consultorio = session.Load<Consultorio>(consultorioId);
                var fechaTurno = queryTurno.GetFechaTurno(editModel.TurnoId);

                var turno = Turno.Create(fechaTurno, paciente, profesional, especialidad, User.As<Secretaria>(), consultorio, editModel.EsTelefonico);

                session.Save(turno);

                //Update del cache
                var query = QueryFactory.Create<IObtenerTurnosDisponiblesPorProfesionalDropDownQuery>();
                query.ProfesionalId = editModel.ProfesionalId.Value;
                query.ClearCache();

                ShowMessages(ResponseMessage.Success("Turno otorgado con éxito."));

                return RedirectToAction("OtorgarTurno");
            }
            return View(editModel);
        }

        [AjaxHandleError]
        public JsonResult GetTurnosDisponiblesProfesional(string especialidadId)
        {
            var queryEspecialidades = QueryFactory.Create<IObtenerEspecialidadesProfesionalDropDownQuery>();
            var queryTurnos = QueryFactory.Create<IObtenerTurnosDisponiblesPorProfesionalDropDownQuery>();
            queryTurnos.ProfesionalId = queryEspecialidades.GetProfesionalId(especialidadId);
            queryTurnos.EspecialidadId = queryEspecialidades.GetEspecialidadId(especialidadId);
            var result = queryTurnos.Execute();
            return Json(result, JsonRequestBehavior.AllowGet);
        }        
        
        [AjaxHandleError]
        public JsonResult GetEspecialidadesProfesional(long profesioanlId)
        {
            var queryEspecialidades = QueryFactory.Create<IObtenerEspecialidadesProfesionalDropDownQuery>();
            queryEspecialidades.ProfesionalId = profesioanlId;            
            var result = queryEspecialidades.Execute();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void AppendLists(OtorgarTurnoEditModel editModel)
        {
            var queryPacientes = QueryFactory.Create<IObtenerPacientesDropDownQuery>();
            queryPacientes.SelectedValue = editModel.PacienteId;
            editModel.PacientesDisponibles = queryPacientes.Execute();
            var queryProfesionales = QueryFactory.Create<IObtenerProfesionalesDropDownQuery>();
            queryProfesionales.SelectedValue = editModel.ProfesionalId;
            editModel.ProfesionalesDisponibles = queryProfesionales.Execute();

            if(editModel.ProfesionalId.HasValue)
            {
                var queryEspecialidades = QueryFactory.Create<IObtenerEspecialidadesProfesionalDropDownQuery>();
                queryEspecialidades.ProfesionalId = editModel.ProfesionalId.Value;
                queryEspecialidades.SelectedValue = editModel.EspecialidadId;
                editModel.EspecialidadesProfesional = queryPacientes.Execute();

                if(!string.IsNullOrWhiteSpace(editModel.EspecialidadId))
                {
                    var queryTurnos = QueryFactory.Create<IObtenerTurnosDisponiblesPorProfesionalDropDownQuery>();
                    queryTurnos.ProfesionalId = editModel.ProfesionalId.Value;
                    queryTurnos.EspecialidadId = queryEspecialidades.GetEspecialidadId(editModel.EspecialidadId);
                    queryTurnos.SelectedValue = editModel.TurnoId;
                    editModel.TurnosDisponibles = queryTurnos.Execute();
                }
            }
        }
        #endregion

        #region Registrar Ingreso

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarIngresoPaciente(long turnoId)
        {
            var session = SessionFactory.GetCurrentSession();
            var turno = session.Get<Turno>(turnoId);
            if (turno == null || !turno.PuedeAplicar(Turno.EventoTurno.Presentar))
            {
                ShowMessages(ResponseMessage.Error("No se encuentra el turno o no se puede marcar su ingreso."));
                return RedirectToAction("Agenda");
            }

            turno.RegistrarIngreso(User.As<Secretaria>());

            ShowMessages(ResponseMessage.Success());
            return RedirectToAction("Agenda");
        }

        #endregion

        #region Alta Paciente

        [HttpGet]
        public ActionResult AltaPaciente()
        {
            var editModel = new AltaPacienteEditModel();
            AppendLists(editModel);
            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AltaPaciente(AltaPacienteEditModel editModel)
        {
            AppendLists(editModel);

            if (ModelState.IsValid)
            {
                var session = SessionFactory.GetCurrentSession();
                // Attempt to register the user                
                var model = _mappingEngine.Map<Persona>(editModel);
                //Update not automapped properties
                model.Documento = new Documento
                {
                    Numero = editModel.DocumentoNumero,
                    TipoDocumento = Enumeration.FromValue<TipoDocumento>(editModel.TipoDocumentoId)
                };
                model.Domicilio = new Domicilio
                {
                    Direccion = editModel.DomicilioDireccion,
                    Localidad = session.Load<Localidad>(editModel.DomicilioLocalidadId)
                };
                var pacienteRol = Paciente.Create(editModel.NumeroAfiliado);
                pacienteRol.Plan = session.Load<Plan>(editModel.PlanId);
                model.AgregarRol(pacienteRol);
                //Seteo un password cualquiera y luego le mando mail re recupero
                var status = _membershipService.CreateUser(model, editModel.Email, Guid.NewGuid().ToString());
                if (status == MembershipStatus.USER_CREATED)
                {
                    _membershipService.RecoverPassword(editModel.Email);
                    ShowMessages(ResponseMessage.Success("Paciente '{0}' registrado correctamente.", model.NombreCompleto));
                    return RedirectToAction("OtorgarTurno");
                }
                ModelState.AddModelError("", status.Get());
            }

            return View(editModel);
        }

        private void AppendLists(AltaPacienteEditModel viewModel)
        {
            viewModel.TiposDocumentosHabilitados = GetTiposDocumentos(viewModel.TipoDocumentoId);
            viewModel.ProvinciasHabilitadas = GetProvincias(viewModel.DomicilioLocalidadProvinciaId);
            viewModel.ObrasSocialesHabilitadas = GetObrasSociales(viewModel.ObraSocialId);

            if (viewModel.DomicilioLocalidadProvinciaId.HasValue)
                viewModel.LocalidadesHabilitadas =
                    GetLocalidadesPorProvincia(viewModel.DomicilioLocalidadProvinciaId.Value, viewModel.DomicilioLocalidadId);

            if (viewModel.ObraSocialId.HasValue)
                viewModel.PlanesObraSocialHabilitados =
                    GetPlanesPorObraSocial(viewModel.ObraSocialId.Value, viewModel.PlanId);
        }
        #endregion
    }
}