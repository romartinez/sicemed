using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;
using Sicemed.Web.Models.ViewModels.Cuenta;
using Sicemed.Web.Models.ViewModels.Paciente;
using Sicemed.Web.Models.ViewModels.Profesional;
using Sicemed.Web.Models.ViewModels.Secretaria;

namespace Sicemed.Web.Infrastructure.AutoMapper
{
    public class GenericProfile : Profile
    {
        public override string ProfileName
        {
            get { return "GenericProfile"; }
        }

        protected override void Configure()
        {
            CreateMap<Clinica, ClinicaViewModel>();

            CreateMap<RegistroPersonaViewModel, Persona>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.Membership, m => m.Ignore())
                .ForMember(d => d.Documento, m => m.Ignore())
                .ForMember(d => d.Domicilio, m => m.Ignore())
                .ForMember(d => d.Roles, m => m.Ignore());

            #region InfoViewModel maps
            CreateMap<Enumeration, InfoViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(o => o.Value))
                .ForMember(d => d.Descripcion, m => m.MapFrom(o => o.DisplayName));

            CreateMap<Consultorio, InfoViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(o => o.Id))
                .ForMember(d => d.Descripcion, m => m.MapFrom(o => o.Nombre));

            CreateMap<Especialidad, InfoViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(o => o.Id))
                .ForMember(d => d.Descripcion, m => m.MapFrom(o => o.Nombre));

            CreateMap<Rol, InfoViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(o => o.Id))
                .ForMember(d => d.Descripcion, m => m.MapFrom(o => o.Persona.NombreCompleto));
            #endregion

            #region Secretaria
            CreateMap<Profesional, TurnosDelDiaViewModel.ProfesionalViewModel>()
                //NOTE: La lleno desde la query, ya que solo los de la fecha
                //muestro.
                .ForMember(d => d.Turnos, m => m.Ignore());
            CreateMap<Turno, TurnosDelDiaViewModel.TurnoViewModel>();

            CreateMap<AltaPacienteEditModel, Persona>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.Membership, m => m.Ignore())
                .ForMember(d => d.Documento, m => m.Ignore())
                .ForMember(d => d.Domicilio, m => m.Ignore())
                .ForMember(d => d.Roles, m => m.Ignore());

            CreateMap<Paciente, SelectListItem>()
                .ForMember(d => d.Selected, m => m.Ignore())
                .ForMember(d => d.Text, m => m.MapFrom(o => 
                    string.Format("{0} - {1} {2}", 
                        o.Persona.NombreCompleto, o.Persona.Documento.TipoDocumento.DisplayName, o.Persona.Documento.Numero)))
                .ForMember(d => d.Value, m => m.MapFrom(o => o.Id));

            CreateMap<Profesional, SelectListItem>()
                .ForMember(d => d.Selected, m => m.Ignore())
                .ForMember(d => d.Text, m => m.MapFrom(o => 
                    string.Format("{0} - {1}", 
                        o.Persona.NombreCompleto, string.Join(", ", o.Especialidades.Select(e=>e.Nombre).ToArray()))))
                .ForMember(d => d.Value, m => m.MapFrom(o => o.Id));
            #endregion

            #region Profesional
            CreateMap<Profesional, AgendaProfesionalViewModel>()
                //NOTE: La lleno desde la query, ya que solo los de la fecha
                //muestro.
                .ForMember(d => d.FechaTurnos, m => m.Ignore())
                .ForMember(d => d.Turnos, m => m.Ignore());
            CreateMap<Turno, AgendaProfesionalViewModel.TurnoViewModel>();
            #endregion

            #region Paciente
            CreateMap<Paciente, AgendaPacienteViewModel>()
                //NOTE: La lleno desde la query, ya que solo los de la fecha
                //muestro.
                .ForMember(d => d.FechaTurnos, m => m.Ignore())
                .ForMember(d => d.Turnos, m => m.Ignore());
            CreateMap<Turno, AgendaPacienteViewModel.TurnoViewModel>();
            #endregion
        }
    }
}