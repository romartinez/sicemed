using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using AutoMapper;
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

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class PersonasController : CrudBaseController<Persona>
    {
        private readonly IMembershipService _membershipService;

        public PersonasController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }


        protected override Expression<Func<Persona, object>> DefaultOrderBy
        {
            get { return x => x.Membership.Email; }
        }

        public override void Editar(long id, string oper, Persona modelo)
        {
            throw new NotSupportedException("Use la ventana de editar.");
        }

        public override void Nuevo(string oper, Persona modelo, int paginaId = 0)
        {
            throw new NotSupportedException("Use la ventana de nuevo.");
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

        #region Nuevo
        public ActionResult Crear()
        {
            var viewModel = new PersonaEditModel();
            AppendLists(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(PersonaEditModel viewModel)
        {
            AppendLists(viewModel);
            if(ModelState.IsValid)
            {
                try
                {
                    var session = SessionFactory.GetCurrentSession();
                    var persona = Mapper.Map<Persona>(viewModel);
                    //Asignaciones que no se puede hacer en el mapper
                    persona.Domicilio = new Domicilio
                    {
                        Direccion = viewModel.DomicilioDireccion,
                        Localidad = session.Load<Localidad>(viewModel.DomicilioLocalidadId)
                    };

                    if (viewModel.EsPaciente)
                    {
                        persona.As<Paciente>().Plan = session.Load<Plan>(viewModel.Paciente.PlanId);
                    }

                    if (viewModel.EsProfesional)
                    {
                        var personaProfesional = persona.As<Profesional>();
                        if (viewModel.Profesional.EspecialidadesSeleccionadas != null)
                        {
                            foreach (var especialidadId in viewModel.Profesional.EspecialidadesSeleccionadas)
                            {
                                var especialidad = session.Load<Especialidad>(especialidadId);
                                personaProfesional.AgregarEspecialidad(especialidad);
                            }
                        }
                        if (viewModel.Profesional.Agendas != null)
                        {
                            foreach (var agendaEditModel in viewModel.Profesional.Agendas)
                            {
                                var agendaModel = Mapper.Map<Agenda>(agendaEditModel);

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
                        }
                    }

                    //Le seteo un password cualquiera, y luego envio mail para que lo resetee
                    var status = _membershipService.CreateUser(persona, viewModel.Email, Guid.NewGuid().ToString());
                    if (status == MembershipStatus.USER_CREATED)
                    {
                        //Envio mail para que cargue su password
                        _membershipService.RecoverPassword(viewModel.Email);
                        ShowMessages(ResponseMessage.Success());
                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("", status.Get());                    
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(viewModel);
        }        

        public ActionResult NuevaAgenda()
        {
            var viewModel = new AgendaEditModel();
            AppendLists(viewModel);
            return PartialView("_Agenda", viewModel);
        }

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
            if(viewModel.Profesional.Agendas != null)
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

        protected override IEnumerable AplicarProjections(IEnumerable<Persona> results)
        {
            return results.Select(x => new
                                       {
                                           x.Documento,
                                           Domicilio = x.Domicilio != null
                                                           ? new
                                                             {
                                                                 x.Domicilio.Direccion,
                                                                 Localidad = x.Domicilio.Localidad != null
                                                                                 ? new
                                                                                   {
                                                                                       x.Domicilio.Localidad.Id,
                                                                                       x.Domicilio.Localidad.Nombre,
                                                                                       Provincia =
                                                                                       x.Domicilio.Localidad.Provincia !=
                                                                                       null
                                                                                           ? new
                                                                                             {
                                                                                                 x.Domicilio.Localidad.
                                                                                                 Provincia.Id,
                                                                                                 x.Domicilio.Localidad.
                                                                                                 Provincia.Nombre
                                                                                             }
                                                                                           : null
                                                                                   }
                                                                                 : null
                                                             }
                                                           : null,
                                           x.Id,
                                           x.NombreCompleto,
                                           x.FechaNacimiento,
                                           x.Telefono,
                                           Roles = x.Roles.Select(r => r.DisplayName),
                                           Membership = x.Membership != null
                                                            ? new
                                                              {
                                                                  x.Membership.Email,
                                                                  x.Membership.IsLockedOut
                                                              }
                                                            : null
                                       });
        }
    }
}