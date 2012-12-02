using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using SICEMED.Web;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Enums;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Queries.ObtenerTurno;
using Sicemed.Web.Infrastructure.Queries.Secretaria;
using Sicemed.Web.Infrastructure.Reports;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels.Secretaria;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Profesional))]
    [AuthorizeIt(typeof(Paciente))]
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
            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OtorgarTurno(OtorgarTurnoEditModel editModel)
        {
            if (ModelState.IsValid)
            {
                var session = SessionFactory.GetCurrentSession();
                var paciente = session.Get<Paciente>(editModel.PacienteId.Value);
                var profesional = session.Get<Profesional>(editModel.ProfesionalId.Value);
                var especialidad = session.Get<Especialidad>(editModel.EspecialidadId);
                var fechaTurno = editModel.FechaTurno;
//RM SE AGREGA DATOS DE LA FORMA DE PAGO AL TURNO
                var plan = paciente.Plan;
                var numeroAfiliado = "";
                var coseguro = decimal.Parse("0.00");

                if (plan != null)
                {
                    numeroAfiliado = paciente.NumeroAfiliado;
                    coseguro = decimal.Parse("0.00");
                    if (paciente.Plan.Coseguro != null)
                    {
                        coseguro = paciente.Plan.Coseguro;
                    }
                    else
                    {
                        coseguro = decimal.Parse("0.00");
                    }

                }

                Turno turno;
                if (!editModel.EsSobreTurno)
                {
                    var consultorio = session.Load<Consultorio>(editModel.ConsultorioId);
//RM SE AGREGA DATOS DE LA FORMA DE PAGO AL TURNO
                    turno = Turno.Create(fechaTurno, editModel.DuracionTurno, paciente, profesional, especialidad, User.As<Secretaria>(), consultorio, plan, numeroAfiliado,coseguro, editModel.EsTelefonico);
                }
                else
                {
//RM SE AGREGA DATOS DE LA FORMA DE PAGO AL TURNO
                    turno = Turno.CreateSobreTurno(fechaTurno, editModel.DuracionTurno, paciente, profesional, especialidad, User.As<Secretaria>(),plan, numeroAfiliado,coseguro, editModel.EsTelefonico);
                }

                session.Save(turno);

                //Update del cache
                var query = QueryFactory.Create<IObtenerTurnosProfesionalQuery>();
                query.ProfesionalId = editModel.ProfesionalId.Value;
                query.ClearCache();

                ShowMessages(ResponseMessage.Success("Turno otorgado con éxito."));
                return RedirectToAction("OtorgarTurno"); 
            }
            return View(editModel);
        }

        [AjaxHandleError]
        public JsonResult GetTurnosProfesional(long profesionalId, long? especialidadId)
        {
            var queryTurnos = QueryFactory.Create<IObtenerTurnosProfesionalQuery>();
            queryTurnos.ProfesionalId = profesionalId;
            queryTurnos.EspecialidadId = especialidadId;
            queryTurnos.AgregarOtorgados = true;
            var turnos = queryTurnos.Execute().ToList();
            var result = new
                {
                    Turnos = turnos,
                    MinimoHorario = turnos.Min(x => x.FechaTurnoInicial.Hour),
                    MaximoHorario = turnos.Max(x => x.FechaTurnoFinal.Hour) + 1, // +1 así ve el final
                    MinimoDuracion = Math.Abs(turnos.Min(x => x.DuracionTurno).TotalMinutes / 2)
                };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleError]
        public JsonResult GetEspecialidadesProfesional(long profesionalId)
        {
            var queryEspecialidades = QueryFactory.Create<IObtenerEspecialidadesProfesionalQuery>();
            queryEspecialidades.ProfesionalId = profesionalId;
            var result = queryEspecialidades.Execute();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Registrar Ingreso

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarIngresoPaciente(long turnoId)
        {
            var session = SessionFactory.GetCurrentSession();
            var turno = session.Get<Turno>(turnoId);
            if (turno == null || !turno.PuedeAplicar(EventoTurno.Presentar))
            {
                ShowMessages(ResponseMessage.Error("No se encuentra el turno o no se puede marcar su ingreso."));
                return RedirectToAction("Agenda");
            }

            turno.RegistrarIngreso(User.As<Secretaria>());

            ShowMessages(ResponseMessage.Success());
            return RedirectToAction("Agenda", new { fecha = turno.FechaTurno.ToShortDateString() });
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
                if (editModel.TipoDocumentoId.HasValue)
                {
                    model.Documento = new Documento
                    {
                        Numero = editModel.DocumentoNumero.Value,
                        TipoDocumento = Enumeration.FromValue<TipoDocumento>(editModel.TipoDocumentoId.Value)
                    };
                }
                model.Domicilio = new Domicilio
                {
                    Direccion = editModel.DomicilioDireccion,
                };
                if (editModel.DomicilioLocalidadId.HasValue)
                {
                    model.Domicilio.Localidad = session.Load<Localidad>(editModel.DomicilioLocalidadId);
                }
                var pacienteRol = Paciente.Create(editModel.NumeroAfiliado);
                if (editModel.PlanId.HasValue) pacienteRol.Plan = session.Load<Plan>(editModel.PlanId);
                model.AgregarRol(pacienteRol);

                if (string.IsNullOrWhiteSpace(model.Membership.Email))
                    model.Membership.Email = string.Format("{0}.{1}@cqr.com.ar", model.Nombre, model.Apellido);

                //Seteo un password cualquiera y luego le mando mail re recupero
                var status = _membershipService.CreateUser(model, model.Membership.Email, Guid.NewGuid().ToString());
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

        #region Edicion Paciente

        [HttpGet]
        public ActionResult EdicionPaciente(long? id = null)
        {
            EdicionPacienteEditModel editModel = null;
            if (id.HasValue)
            {
                var paciente = SessionFactory.GetCurrentSession().Load<Paciente>(id.Value);
                if (paciente != null)
                {
                    editModel = MappingEngine.Map<EdicionPacienteEditModel>(paciente.Persona);
                    if (paciente.Plan != null)
                    {
                        editModel.PlanId = paciente.Plan.Id;
                        editModel.ObraSocialId = paciente.Plan.ObraSocial.Id;
                    }
                    if (paciente.Persona.Documento != null && paciente.Persona.Documento.TipoDocumento != null)
                    {
                        editModel.TipoDocumentoId = paciente.Persona.Documento.TipoDocumento.Value;
                    }
                    if (paciente.Persona.Domicilio != null && paciente.Persona.Domicilio.Localidad != null)
                    {
                        editModel.DomicilioLocalidadProvinciaId = paciente.Persona.Domicilio.Localidad.Provincia.Id;
                        editModel.DomicilioLocalidadId = paciente.Persona.Domicilio.Localidad.Id;
                    }
                    editModel.NumeroAfiliado = paciente.NumeroAfiliado;
                }
            }

            if (editModel == null) editModel = new EdicionPacienteEditModel();

            AppendLists(editModel);
            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EdicionPaciente(EdicionPacienteEditModel editModel)
        {
            AppendLists(editModel);

            if (ModelState.IsValid)
            {
                var session = SessionFactory.GetCurrentSession();
                var paciente = session.Load<Paciente>(editModel.Id);
                
                // Attempt to register the user                
                var model = _mappingEngine.Map(editModel, paciente.Persona);
                //Update not automapped properties
                model.Membership.Email = editModel.Email;
                if (editModel.PlanId.HasValue)
                {
                    paciente.Plan = session.Load<Plan>(editModel.PlanId.Value);
                }
                else
                {
                    paciente.Plan = null;
                }
                paciente.NumeroAfiliado = editModel.NumeroAfiliado;
                if (editModel.TipoDocumentoId.HasValue || editModel.DocumentoNumero.HasValue)
                {
                    model.Documento = new Documento();
                    if (editModel.DocumentoNumero.HasValue) model.Documento.Numero = editModel.DocumentoNumero.Value;
                    model.Documento.TipoDocumento = Enumeration.FromValue<TipoDocumento>(editModel.TipoDocumentoId.Value);
                }
                else
                {
                    model.Documento = null;
                }
                model.Domicilio = new Domicilio
                {
                    Direccion = editModel.DomicilioDireccion,
                };
                if (editModel.DomicilioLocalidadId.HasValue)
                {
                    model.Domicilio.Localidad = session.Load<Localidad>(editModel.DomicilioLocalidadId);
                }
                else
                {
                    model.Domicilio.Localidad = null;
                }

                if (string.IsNullOrWhiteSpace(model.Membership.Email))
                    model.Membership.Email = string.Format("{0}.{1}@cqr.com.ar", model.Nombre, model.Apellido);

                ShowMessages(ResponseMessage.Success("Paciente '{0}' modificado correctamente.", model.NombreCompleto));

                if (_membershipService.GetCurrentUser().IsInRole(Rol.SECRETARIA))
                { return RedirectToAction("OtorgarTurno"); }
                else
                {
                    if (_membershipService.GetCurrentUser().IsInRole(Rol.PROFESIONAL))
                    { return RedirectToAction("Agenda", "Profesional"); }
                    else
                    { return RedirectToAction("Agenda", "Paciente"); }
                }
            }

            return View(editModel);
        }

        private void AppendLists(EdicionPacienteEditModel viewModel)
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

        #region Edicion Obra Social Turno

        [HttpGet]
        public ActionResult EditarObraSocialTurno(long? id = null)
        {
            EdicionObraSocialTurnoEditModel editModel = null;
                        if (id.HasValue)
                        {
                            var turno = SessionFactory.GetCurrentSession().Load<Turno>(id.Value);
                            if (turno != null)
                            {
                                editModel = MappingEngine.Map<EdicionObraSocialTurnoEditModel>(turno.Plan);
                                if (turno.Plan != null)
                                {
                                    editModel.PlanId = turno.Plan.Id;
                                    editModel.ObraSocialId = turno.Plan.ObraSocial.Id;
                                    editModel.NumeroAfiliado = turno.NumeroAfiliado;
                                }
                            }
                        }
            
            if (editModel == null) editModel = new EdicionObraSocialTurnoEditModel();
//            var editModel = new EdicionObraSocialTurnoEditModel();
            AppendLists(editModel);
            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarObraSocialTurno(EdicionObraSocialTurnoEditModel editModel)
        {
            AppendLists(editModel);

            if (ModelState.IsValid)
            {
                var session = SessionFactory.GetCurrentSession();
                var turno = session.Load<Turno>(editModel.Id);
                var model = _mappingEngine.Map(editModel, turno.Plan);
                
                var planParticular = session.Load<Plan>(long.Parse(GetPlanParticular(null).First(x => x.Selected).Value));

                
//Modifica Plan Obra Social
                if (editModel.PlanId.HasValue)
                {
                    turno.Plan = session.Load<Plan>(editModel.PlanId.Value);
                }
                else
                {
//RM: OS Consulta Particular para que sea el default en caso de que no haya ningun plan elegido
                    //turno.Plan = null;
                    turno.Plan=planParticular;
                }

//Modifica Numero Afiliado
                if ((string.IsNullOrWhiteSpace(editModel.NumeroAfiliado)) || turno.Plan.Nombre.Equals("Consulta Particular"))
                    {turno.NumeroAfiliado = "999999";}
                else
                    {turno.NumeroAfiliado = editModel.NumeroAfiliado;}

//Modifica Coseguro si es OS o la tarifa del profesional si es consulta particular
                if (turno.Plan.Nombre.Equals("Consulta Particular"))
                    {
                        if (turno.Profesional.RetencionFija.HasValue)
                            { turno.Coseguro = turno.Profesional.RetencionFija.Value; }
                        else
                            { turno.Coseguro = 0; }
                    }
                else
                    {
                        turno.Coseguro = turno.Plan.Coseguro;
                    }

                ShowMessages(ResponseMessage.Success("Turno modificado correctamente."));
                return RedirectToAction("Agenda");
            }

            return View(editModel);
        }

        private void AppendLists(EdicionObraSocialTurnoEditModel viewModel)
        {
            viewModel.ObrasSocialesHabilitadas = GetObrasSociales(viewModel.ObraSocialId);
            if (viewModel.ObraSocialId.HasValue)
                viewModel.PlanesObraSocialHabilitados =
                    GetPlanesPorObraSocial(viewModel.ObraSocialId.Value, viewModel.PlanId);
        }
        #endregion

        #region Reportes
        public ActionResult ReporteTurnos(DateTime? fecha = null)
        {
            var reportEngine = new ReportEngine();
            var report = ReportFactory.Create<ITurnosReporte>();
            report.Fecha = fecha.HasValue ? fecha.Value : DateTime.Now;
            var reportBytes = reportEngine.BuildReport(report);
            return File(reportBytes, "application/pdf", string.Format("TurnosDia_{0:yy-MM-dd}.pdf", report.Fecha));
        }

        #endregion
    }
}