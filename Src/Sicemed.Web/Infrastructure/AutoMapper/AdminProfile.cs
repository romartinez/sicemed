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
            Mapper.CreateMap<Clinica, ClinicaEditViewModel>()
                .ForMember(d => d.TiposDocumentosHabilitados, m => m.Ignore())
                .ForMember(d => d.LocalidadesHabilitadas, m => m.Ignore())
                .ForMember(d => d.ProvinciasHabilitadas, m => m.Ignore());

            Mapper.CreateMap<ClinicaEditViewModel, Clinica>()
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

            Mapper.CreateMap<PersonaEditModel, Persona>()
                .ForMember(d => d.Documento, m => m.ResolveUsing(o => 
                    new Documento
                    {
                        Numero = o.DocumentoNumero,
                        TipoDocumento = Enumeration.FromValue<TipoDocumento>(o.TipoDocumentoId)
                }))
                .ForMember(d => d.Domicilio, m => m.Ignore())
                .AfterMap(PersonaEditModelToPersonaAfterMap);
            Mapper.CreateMap<PacienteEditModel, Paciente>()
                .ForMember(d => d.FechaAsignacion, m => m.Ignore())
                .ForMember(d => d.Plan, m => m.Ignore())
                .ForMember(d => d.Persona, m => m.Ignore());
            Mapper.CreateMap<SecretariaEditModel, Secretaria>()
                .ForMember(d => d.FechaAsignacion, m => m.Ignore())
                .ForMember(d => d.Persona, m => m.Ignore());
            Mapper.CreateMap<ProfesionalEditModel, Profesional>()
                .ForMember(d => d.FechaAsignacion, m => m.Ignore())
                //Mapeo a mano las agendas porque referencian otras entidades
                .ForMember(d => d.Agendas, m => m.Ignore()) 
                .ForMember(d => d.Persona, m => m.Ignore());
            Mapper.CreateMap<AgendaEditModel, Agenda>()
                .ForMember(d => d.Consultorio, m => m.Ignore())
                .ForMember(d => d.Profesional, m => m.Ignore());
        }

        private static void PersonaEditModelToPersonaAfterMap(PersonaEditModel editModel, Persona model)
        {
            UpdateRole<Paciente>(model, editModel.EsPaciente, editModel.Paciente);
            UpdateRole<Secretaria>(model, editModel.EsSecretaria, editModel.Secretaria);
            UpdateRole<Profesional>(model, editModel.EsProfesional, editModel.Profesional);
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

        private static void UpdateRole<T>(Persona persona, bool editModelIsSelected, object editModel) where T : Rol
        {
            if (editModelIsSelected && persona.IsInRole<T>())
            {
                //Update current values
                var rol = persona.As<T>();
                Mapper.Map(editModel, rol);
            }
            else
            {
                if (editModelIsSelected && !persona.IsInRole<T>())
                {
                    var rol = Mapper.Map<T>(editModel);
                    persona.AgregarRol(rol);
                }
                else if (!editModelIsSelected && persona.IsInRole<T>())
                {
                    var rol = persona.As<T>();
                    persona.QuitarRol(rol);
                }
            }
        }
    }
}