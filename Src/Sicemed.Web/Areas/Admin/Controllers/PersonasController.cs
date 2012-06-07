using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using NHibernate.Transform;
using Sicemed.Web.Areas.Admin.Models.Personas;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Enums;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    [AuthorizeIt(typeof(Administrador))]
    public class PersonasController : NHibernateController
    {
        private readonly IMembershipService _membershipService;
        private readonly IMappingEngine _mappingEngine;

        public PersonasController(IMembershipService membershipService, IMappingEngine mappingEngine)
        {
            _membershipService = membershipService;
            _mappingEngine = mappingEngine;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual JsonResult List(long count, int page, int rows)
        {
            page--;
            var session = SessionFactory.GetCurrentSession();
            var query = session.QueryOver<Persona>()                
                .Fetch(x => x.Domicilio.Localidad).Eager
                .Fetch(x => x.Domicilio.Localidad.Provincia).Eager
                .Fetch(x => x.Roles).Eager
                .OrderBy(x => x.Membership.Email).Asc
                .TransformUsing(Transformers.DistinctRootEntity);

            var respuesta = new PaginableResponse();

            if (page == 0)
            {
                var queryCount = query.ToRowCountInt64Query().FutureValue<long>();
                respuesta.Records = queryCount.Value;
            }
            else
            {
                respuesta.Records = count;
            }
            var entites = query.Skip(page * rows).Take(rows).Future();
            respuesta.Rows = _mappingEngine.Map<IEnumerable<PersonaViewModel>>(entites);

            respuesta.Page = ++page;
            respuesta.Total = (long)Math.Ceiling(respuesta.Records / (double)rows);
            return Json(respuesta);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public void BloquearUsuario(long usuarioId)
        {
            var session = SessionFactory.GetCurrentSession();
            var user = session.Get<Persona>(usuarioId);
            if (user == null) throw new ValidationErrorException("El usuario no existe.");
            if (user.Membership.IsLockedOut)
                throw new ValidationErrorException("El usuario ya se encuentra bloqueado.");

            MembershipService.LockUser(user.Membership.Email,
                                       string.Format("{0:dd/mm/yy} - {1} - Bloqueo Administrativo", DateTime.Now, User));

            ShowMessages(ResponseMessage.Success("Bloqueo realizado con éxito."));
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public void DesbloquearUsuario(long usuarioId)
        {
            var session = SessionFactory.GetCurrentSession();
            var user = session.Get<Persona>(usuarioId);
            if (user == null) throw new ValidationErrorException("El usuario no existe.");
            if (!user.Membership.IsLockedOut)
                throw new ValidationErrorException("El usuario ya se encuentra desbloqueado.");

            MembershipService.UnlockUser(user.Membership.Email);

            ShowMessages(ResponseMessage.Success("Desbloqueo realizado con éxito."));
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public void EnviarPasswordReset(long usuarioId)
        {
            var session = SessionFactory.GetCurrentSession();
            var user = session.Get<Persona>(usuarioId);
            if (user == null) throw new ValidationErrorException("El usuario no existe.");

            MembershipService.RecoverPassword(user.Membership.Email);

            ShowMessages(ResponseMessage.Success("Se ha enviado el mail con los pasos para recuperar el password a '{0}'.", user.Membership.Email));
        }

        #region Nuevo
        public ActionResult Nuevo()
        {
            var viewModel = new PersonaEditModel();
            AppendLists(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Nuevo(PersonaEditModel editModel)
        {
            AppendLists(editModel);
            if (ModelState.IsValid)
            {
                try
                {
                    var persona = _mappingEngine.Map<Persona>(editModel);
                    AdditionalMappings(editModel, persona);

                    //Le seteo un password cualquiera, y luego envio mail para que lo resetee
                    var status = _membershipService.CreateUser(persona, editModel.Email, Guid.NewGuid().ToString());
                    if (status == MembershipStatus.USER_CREATED)
                    {
                        //Envio mail para que cargue su password
                        _membershipService.RecoverPassword(editModel.Email);
                        ShowMessages(ResponseMessage.Success());
                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("", status.Get());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(editModel);
        }

        public ActionResult NuevaAgenda()
        {
            var viewModel = new AgendaEditModel();
            AppendLists(viewModel);
            return PartialView("_Agenda", viewModel);
        }
        #endregion

        #region Editar
        [HttpGet]
        [AjaxHandleError]
        public ActionResult Editar(long personaId)
        {
            var model = GetPersona(personaId);
            if (model == null)
            {
                ShowMessages(ResponseMessage.Error("No se ha encontrado el usuario."));
                return RedirectToAction("Index");
            }

            var editModel = _mappingEngine.Map<PersonaEditModel>(model);
            AppendLists(editModel);
            return View(editModel);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(PersonaEditModel editModel)
        {
            AppendLists(editModel);
            if (ModelState.IsValid)
            {
                try
                {
                    var session = SessionFactory.GetCurrentSession();
                    var persona = GetPersona(editModel.Id.Value);
                    if (persona == null)
                    {
                        ShowMessages(ResponseMessage.Error("No se ha encontrado el usuario."));
                        return RedirectToAction("Index");
                    }

                    _mappingEngine.Map(editModel, persona);
                    AdditionalMappings(editModel, persona);

                    ShowMessages(ResponseMessage.Success());
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.ToString());
                }
                return View(editModel);
            }
            return View(editModel);
        }

        private void AdditionalMappings(PersonaEditModel editModel, Persona persona)
        {
            var session = SessionFactory.GetCurrentSession();
            persona.Domicilio = new Domicilio
                                    {
                                        Direccion = editModel.DomicilioDireccion,
                                        Localidad = session.Load<Localidad>(editModel.DomicilioLocalidadId)
                                    };

            if (editModel.EsPaciente)
            {
                persona.As<Paciente>().Plan = session.Load<Plan>(editModel.Paciente.PlanId);
            }

            if (editModel.EsProfesional)
            {
                var personaProfesional = persona.As<Profesional>();
                ProcesarEspecialidades(editModel, personaProfesional);
                ProcesarAgendas(editModel, personaProfesional);
            }
        }

        private void ProcesarEspecialidades(PersonaEditModel editModel, Profesional personaProfesional)
        {
            var session = SessionFactory.GetCurrentSession();
            //Especialidades a eliminar
            var especialidadesAEliminar =
                personaProfesional.Especialidades.Where(
                    x => !editModel.Profesional.EspecialidadesSeleccionadas.Contains(x.Id)).ToList();
            foreach (var especialidadAEliminar in especialidadesAEliminar)
            {
                personaProfesional.QuitarEspecialidad(especialidadAEliminar);
            }
            //Especialidades a agregar
            var especialidadesAAgregar =
                editModel.Profesional.EspecialidadesSeleccionadas.Where(
                    x => !personaProfesional.Especialidades.Select(p => p.Id).Contains(x)).ToList();
            foreach (var especialidadAAgregar in especialidadesAAgregar)
            {
                var especialidad = session.Load<Especialidad>(especialidadAAgregar);
                personaProfesional.AgregarEspecialidad(especialidad);
            }
        }

        private void ProcesarEspecialidades(AgendaEditModel editModel, Agenda agenda)
        {
            var session = SessionFactory.GetCurrentSession();
            //Especialidades a eliminar
            var especialidadesAEliminar =
                agenda.EspecialidadesAtendidas.Where(
                    x => !editModel.EspecialidadesSeleccionadas.Contains(x.Id)).ToList();
            foreach (var especialidadAEliminar in especialidadesAEliminar)
            {
                agenda.QuitarEspecialidad(especialidadAEliminar);
            }
            //Especialidades a agregar
            var especialidadesAAgregar =
                editModel.EspecialidadesSeleccionadas.Where(
                    x => !agenda.EspecialidadesAtendidas.Select(p => p.Id).Contains(x)).ToList();
            foreach (var especialidadAAgregar in especialidadesAAgregar)
            {
                var especialidad = session.Load<Especialidad>(especialidadAAgregar);
                agenda.AgregarEspecialidad(especialidad);
            }
        }

        private void ProcesarAgendas(PersonaEditModel editModel, Profesional personaProfesional)
        {
            var session = SessionFactory.GetCurrentSession();
            //Veo el tema de las agendas
            if (editModel.Profesional.Agendas == null || !editModel.Profesional.Agendas.Any())
            {
                personaProfesional.Agendas.Clear();
            }
            else
            {
                //Agendas a eliminar
                var agendasAEliminar =
                    personaProfesional.Agendas.Where(m => !editModel.Profesional.Agendas.Select(x => x.Id).Contains(m.Id)).ToList();
                foreach (var agenda in agendasAEliminar)
                {
                    personaProfesional.QuitarAgenda(agenda);
                }
                //Agendas a agregar
                var agendasAAgregar = editModel.Profesional.Agendas.Where(x => !x.Id.HasValue);
                foreach (var agendaEditModel in agendasAAgregar)
                {
                    var agendaModel = _mappingEngine.Map<Agenda>(agendaEditModel);

                    if (agendaEditModel.EspecialidadesSeleccionadas != null)
                    {
                        foreach (var especialidadId in agendaEditModel.EspecialidadesSeleccionadas)
                        {
                            var especialidad = session.Load<Especialidad>(especialidadId);
                            agendaModel.AgregarEspecialidad(especialidad);
                        }
                    }
                    agendaModel.Consultorio = session.Load<Consultorio>(agendaEditModel.ConsultorioId);

                    personaProfesional.AgregarAgenda(agendaModel);
                }
                //Agendas a actualizar
                var agendasActualizar =
                    personaProfesional.Agendas.Where(m => editModel.Profesional.Agendas.Select(x => x.Id).Contains(m.Id)).ToList();
                foreach (var agenda in agendasActualizar)
                {
                    var agendaEditModel = editModel.Profesional.Agendas.Single(v => v.Id == agenda.Id);
                    _mappingEngine.Map(agendaEditModel, agenda);
                    ProcesarEspecialidades(agendaEditModel, agenda);
                    agenda.Consultorio = session.Load<Consultorio>(agendaEditModel.ConsultorioId);
                }
            }
        }

        #endregion

        private Persona GetPersona(long personaId)
        {
            var session = SessionFactory.GetCurrentSession();
            var persona = session.QueryOver<Persona>().Where(p => p.Id == personaId)
                .Fetch(p => p.Roles).Eager
                .Fetch(p => p.Domicilio.Localidad).Eager
                .Fetch(p => p.Domicilio.Localidad.Provincia).Eager
                .Fetch(p => ((Profesional)p.Roles.First()).Agendas).Eager
                .Fetch(p => ((Profesional)p.Roles.First()).Agendas.First().EspecialidadesAtendidas).Eager
                .Fetch(p => ((Profesional)p.Roles.First()).Especialidades).Eager
                .Fetch(p => ((Paciente)p.Roles.First()).Plan).Eager
                .SingleOrDefault();
            return persona;
        }

        #region Agregar Referencias
        private void AppendLists(PersonaEditModel viewModel)
        {
            viewModel.TiposDocumentosHabilitados = DomainExtensions.GetTiposDocumentos(viewModel.TipoDocumentoId);
            viewModel.ProvinciasHabilitadas = DomainExtensions.GetProvincias(SessionFactory, viewModel.DomicilioLocalidadProvinciaId);

            if (viewModel.DomicilioLocalidadProvinciaId.HasValue)
                viewModel.LocalidadesHabilitadas =
                    DomainExtensions.GetLocalidades(SessionFactory, viewModel.DomicilioLocalidadProvinciaId.Value, viewModel.DomicilioLocalidadId);

            //Paciente
            viewModel.Paciente.ObrasSocialesHabilitadas = DomainExtensions.GetObrasSociales(SessionFactory, viewModel.Paciente.ObraSocialId);
            if (viewModel.Paciente.ObraSocialId.HasValue)
                viewModel.Paciente.PlanesObraSocialHabilitados =
                    DomainExtensions.GetPlanesObraSocial(SessionFactory, viewModel.Paciente.ObraSocialId.Value, viewModel.Paciente.PlanId);

            //Profesional
            viewModel.Profesional.Especialidades = DomainExtensions.GetEspecialidades(SessionFactory, viewModel.Profesional.EspecialidadesSeleccionadas);
            if (viewModel.Profesional.Agendas != null)
            {
                foreach (var agenda in viewModel.Profesional.Agendas)
                {
                    AppendLists(agenda);
                }
            }
        }

        private void AppendLists(AgendaEditModel viewModel)
        {
            viewModel.Especialidades = DomainExtensions.GetEspecialidades(SessionFactory, viewModel.EspecialidadesSeleccionadas);
            viewModel.Consultorios = DomainExtensions.GetConsultorios(SessionFactory, viewModel.ConsultorioId);
        }
        #endregion
    }
}