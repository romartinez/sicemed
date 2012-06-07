using System.Linq;
using AutoMapper;
using Castle.Core;
using Sicemed.Web.Areas.Admin.Models.Clinicas;
using Sicemed.Web.Areas.Admin.Models.Personas;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.AutoMapper
{
    public class AdminProfile : Profile
    {
        public override string ProfileName
        {
            get { return "AdminProfile"; }
        }

        protected override void Configure()
        {
            #region Clinica
            CreateMap<Clinica, ClinicaEditViewModel>()
                .ForMember(d => d.TiposDocumentosHabilitados, m => m.Ignore())
                .ForMember(d => d.LocalidadesHabilitadas, m => m.Ignore())
                .ForMember(d => d.ProvinciasHabilitadas, m => m.Ignore());

            CreateMap<ClinicaEditViewModel, Clinica>()
                .ForMember(d => d.Telefonos, m => m.Ignore())
                .ForMember(d => d.Domicilio, m => m.Ignore())
                .ForMember(d => d.ObrasSocialesAtendidas, m => m.Ignore())
                .ForMember(d => d.Pacientes, m => m.Ignore())
                .ForMember(d => d.Profesionales, m => m.Ignore())
                .ForMember(d => d.Secretarias, m => m.Ignore())
                .ForMember(d => d.Partners, m => m.Ignore())
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.Documento, m => m.Ignore())
                .AfterMap((v, m) =>
                              {
                                  //NOTE: m.Telefonos.Clear(); no funciona!
                                  var telefonosAQuitar = m.Telefonos.ToList();
                                  telefonosAQuitar.ForEach(t => m.QuitarTelefono(t));
                                  v.Telefonos.ForEach(t => m.AgregarTelefono(t));
                              });
            #endregion

            #region Persona -> PersonaViewModel
            CreateMap<Persona, PersonaViewModel>()
                .ForMember(d => d.Roles, m => m.ResolveUsing(o => o.Roles.Select(r => r.DisplayName)));
            #endregion

            #region Persona -> PersonaEditModel
            CreateMap<Persona, PersonaEditModel>()
                .ForMember(d => d.Email, m => m.MapFrom(o => o.Membership.Email))
                .ForMember(d => d.TipoDocumentoId, m => m.MapFrom(o => o.Documento.TipoDocumento.Value))
                .ForMember(d => d.TiposDocumentosHabilitados, m => m.Ignore())
                .ForMember(d => d.ProvinciasHabilitadas, m => m.Ignore())
                .ForMember(d => d.LocalidadesHabilitadas, m => m.Ignore())

                .ForMember(d => d.EsPaciente, m => m.MapFrom(o => o.IsInRole<Paciente>()))
                .ForMember(d => d.Paciente, m =>
                {
                    m.ResolveUsing(o => o.IsInRole<Paciente>() ? o.As<Paciente>() : null);
                    m.NullSubstitute(new PacienteEditModel());
                })

                .ForMember(d => d.EsSecretaria, m => m.MapFrom(o => o.IsInRole<Secretaria>()))
                .ForMember(d => d.Secretaria, m =>
                {
                    m.ResolveUsing(o => o.IsInRole<Secretaria>() ? o.As<Secretaria>() : null);
                    m.NullSubstitute(new SecretariaEditModel());
                })

                .ForMember(d => d.EsProfesional, m => m.MapFrom(o => o.IsInRole<Profesional>()))
                .ForMember(d => d.Profesional, m =>
                {
                    m.ResolveUsing(o => o.IsInRole<Profesional>() ? o.As<Profesional>() : null);
                    m.NullSubstitute(new ProfesionalEditModel());
                })


                .ForMember(d => d.EsAdmin, m => m.MapFrom(o => o.IsInRole<Administrador>()));

            CreateMap<Paciente, PacienteEditModel>()
                .ForMember(d => d.ObraSocialId, m => m.MapFrom(o => o.Plan.ObraSocial.Id))
                .ForMember(d => d.ObrasSocialesHabilitadas, m => m.Ignore())
                .ForMember(d => d.PlanesObraSocialHabilitados, m => m.Ignore());

            CreateMap<Secretaria, SecretariaEditModel>();

            CreateMap<Profesional, ProfesionalEditModel>()
                .ForMember(d => d.EspecialidadesSeleccionadas, m => m.MapFrom(o => o.Especialidades.Select(e => e.Id)))
                .ForMember(d => d.Especialidades, m => m.Ignore());

            CreateMap<Agenda, AgendaEditModel>()
                .ForMember(d => d.EspecialidadesSeleccionadas, m => m.MapFrom(o => o.EspecialidadesAtendidas.Select(e => e.Id)))
                .ForMember(d => d.Consultorios, m => m.Ignore())
                .ForMember(d => d.Especialidades, m => m.Ignore());
            #endregion

            #region PersonaEditModel -> Persona
            CreateMap<PersonaEditModel, Persona>()
                .ForMember(d => d.Documento, m => m.ResolveUsing(o =>
                    new Documento
                    {
                        Numero = o.DocumentoNumero,
                        TipoDocumento = Enumeration.FromValue<TipoDocumento>(o.TipoDocumentoId)
                    }))
                .ForMember(d => d.Domicilio, m => m.Ignore())
                .AfterMap(PersonaEditModelToPersonaAfterMap);
            CreateMap<PacienteEditModel, Paciente>()
                .ForMember(d => d.FechaAsignacion, m => m.Ignore())
                .ForMember(d => d.Plan, m => m.Ignore())
                .ForMember(d => d.Persona, m => m.Ignore());
            CreateMap<SecretariaEditModel, Secretaria>()
                .ForMember(d => d.FechaAsignacion, m => m.Ignore())
                .ForMember(d => d.Persona, m => m.Ignore());
            CreateMap<ProfesionalEditModel, Profesional>()
                .ForMember(d => d.FechaAsignacion, m => m.Ignore())
                //Mapeo a mano las agendas porque referencian otras entidades
                .ForMember(d => d.Agendas, m => m.Ignore())
                .ForMember(d => d.Persona, m => m.Ignore());
            CreateMap<AgendaEditModel, Agenda>()
                .ForMember(d => d.Consultorio, m => m.Ignore())
                .ForMember(d => d.Profesional, m => m.Ignore());
            #endregion
        }

        #region Helpers PersonaEditModel -> Persona
        private void PersonaEditModelToPersonaAfterMap(PersonaEditModel editModel, Persona model)
        {
            UpdateRole<PacienteEditModel, Paciente>(model, editModel.EsPaciente, editModel.Paciente);
            UpdateRole<SecretariaEditModel, Secretaria>(model, editModel.EsSecretaria, editModel.Secretaria);
            UpdateRole<ProfesionalEditModel, Profesional>(model, editModel.EsProfesional, editModel.Profesional);
            //Admin
            if (editModel.EsAdmin && model.IsInRole<Administrador>()) return;
            if (editModel.EsAdmin && !model.IsInRole<Administrador>())
            {
                var rol = Administrador.Create();
                model.AgregarRol(rol);
            }
            else if (!editModel.EsAdmin && model.IsInRole<Administrador>())
            {
                var rol = model.As<Administrador>();
                model.QuitarRol(rol);
            }
        }

        private void UpdateRole<TEditModel, TRol>(Persona persona, bool editModelIsSelected, TEditModel editModel) where TRol : Rol
        {
            //TODO: Faltaria quitar el "Mapper." de aqui pero no es TAN importante
            if (editModelIsSelected && persona.IsInRole<TRol>())
            {
                //Update current values
                var rol = persona.As<TRol>();
                Mapper.Map(editModel, rol);
            }
            else
            {
                if (editModelIsSelected && !persona.IsInRole<TRol>())
                {
                    var rol = Mapper.Map<TRol>(editModel);
                    persona.AgregarRol(rol);
                }
                else if (!editModelIsSelected && persona.IsInRole<TRol>())
                {
                    var rol = persona.As<TRol>();
                    persona.QuitarRol(rol);
                }
            }
        }
        #endregion
    }
}