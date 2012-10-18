using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;
using Sicemed.Web.Models.ViewModels.Cuenta;
using Sicemed.Web.Models.ViewModels.Historial;
using Sicemed.Web.Models.ViewModels.ObtenerTurno;
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

            CreateMap<Turno, TurnoViewModel>()
                .ForMember(x => x.FechaTurnoInicial, m => m.MapFrom(o => o.FechaTurno))
                .ForMember(x => x.DuracionTurno, m => m.MapFrom(o => o.DuracionTurno))
                .ForMember(x => x.EspecialidadesAtendidas, m => m.ResolveUsing(t => new List<Especialidad> { t.Especialidad }));


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

            CreateMap<Persona, InfoViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(o => o.Id))
                .ForMember(d => d.Descripcion, m => m.MapFrom(o => o.NombreCompleto));

            CreateMap<Paciente, PersonaViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(o => o.Id))
                .ForMember(d => d.TipoDocumento, m => m.MapFrom(o => o.Persona.Documento.TipoDocumento.DisplayName))
                .ForMember(d => d.Documento, m => m.MapFrom(o => o.Persona.Documento.Numero))
                .ForMember(d => d.NombreCompleto, m => m.MapFrom(o => o.Persona.NombreCompleto));

            CreateMap<Profesional, ProfesionalConEspecialidadesViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(o => o.Id))
                .ForMember(d => d.TipoDocumento, m => m.MapFrom(o => o.Persona.Documento.TipoDocumento.DisplayName))
                .ForMember(d => d.Documento, m => m.MapFrom(o => o.Persona.Documento.Numero))
                .ForMember(d => d.Especialidades, m => m.MapFrom(o => o.Especialidades))
                .ForMember(d => d.NombreCompleto, m => m.MapFrom(o => o.Persona.NombreCompleto));
            #endregion

            #region Secretaria
            CreateMap<Profesional, TurnosDelDiaViewModel.ProfesionalViewModel>()
                //NOTE: La lleno desde la query, ya que solo los de la fecha
                //muestro.
                .ForMember(d => d.Turnos, m => m.Ignore());
            CreateMap<Turno, TurnosDelDiaViewModel.TurnoViewModel>()
                .ForMember(x => x.PuedoAtender, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Atender)))
                .ForMember(x => x.PuedoCancelar, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Cancelar)))
                .ForMember(x => x.PuedoPresentar, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Presentar)));

            CreateMap<AltaPacienteEditModel, Persona>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.Membership, m => m.Ignore())
                .ForMember(d => d.Documento, m => m.Ignore())
                .ForMember(d => d.Domicilio, m => m.Ignore())
                .ForMember(d => d.Roles, m => m.Ignore());

            CreateMap<Persona, EdicionPacienteEditModel>()
                .ForMember(d => d.TiposDocumentosHabilitados, m => m.Ignore())
                .ForMember(d => d.TipoDocumentoId, m => m.Ignore())
                .ForMember(d => d.Email, m => m.MapFrom(o => o.Membership.Email))
                .ForMember(d => d.ProvinciasHabilitadas, m => m.Ignore())
                .ForMember(d => d.LocalidadesHabilitadas, m => m.Ignore())
                .ForMember(d => d.ObrasSocialesHabilitadas, m => m.Ignore())
                .ForMember(d => d.PlanesObraSocialHabilitados, m => m.Ignore())
                .ForMember(d => d.PlanId, m => m.Ignore())
                .ForMember(d => d.ObraSocialId, m => m.Ignore())
                .ForMember(d => d.NumeroAfiliado, m => m.Ignore());

            CreateMap<EdicionPacienteEditModel, Persona>()                
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
                        o.Persona.NombreCompleto, string.Join(", ", o.Especialidades.Select(e => e.Nombre).ToArray()))))
                .ForMember(d => d.Value, m => m.MapFrom(o => o.Id));
            #endregion

            #region Profesional
            CreateMap<Profesional, AgendaProfesionalViewModel>()
                //NOTE: La lleno desde la query, ya que solo los de la fecha
                //muestro.
                .ForMember(d => d.FechaTurnos, m => m.Ignore())
                .ForMember(d => d.Turnos, m => m.Ignore());
            CreateMap<Turno, AgendaProfesionalViewModel.TurnoViewModel>()
                .ForMember(d => d.Paciente, m => m.Ignore())
                .ForMember(x => x.PuedoCancelar, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Cancelar)));

            CreateMap<Profesional, CalendarioProfesionalViewModel>()
                //NOTE: La lleno desde la query, ya que solo los de la fecha
                //muestro.
                .ForMember(d => d.Turnos, m => m.Ignore());
            CreateMap<Turno, CalendarioProfesionalViewModel.TurnoViewModel>();
            #endregion

            #region Paciente
            CreateMap<Paciente, AgendaPacienteViewModel>()
                //NOTE: La lleno desde la query, ya que solo los de la fecha
                //muestro.
                .ForMember(d => d.FechaTurnos, m => m.Ignore())
                .ForMember(d => d.Turnos, m => m.Ignore());
            CreateMap<Turno, AgendaPacienteViewModel.TurnoViewModel>()
                .ForMember(x => x.PuedoCancelar, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Cancelar)));
            #endregion

            #region Historial

            CreateMap<Turno, AtencionesViewModel.HistorialItem>()
                .ForMember(d => d.Consultorio, m => m.MapFrom(o => o.Consultorio.Nombre))
                .ForMember(d => d.Profesional, m => m.MapFrom(o => o.Profesional.Persona.NombreCompleto))
                .ForMember(d => d.Especialidad, m => m.MapFrom(o => o.Especialidad.Nombre))
                .ForMember(x => x.PuedoAtender, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Atender)))
                .ForMember(x => x.PuedoCancelar, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Cancelar)))
                .ForMember(x => x.PuedoPresentar, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Presentar)));

            CreateMap<Turno, TurnosPacienteViewModel.HistorialItem>()
                .ForMember(d => d.Consultorio, m => m.MapFrom(o => o.Consultorio.Nombre))
                .ForMember(d => d.Profesional, m => m.MapFrom(o => o.Profesional.Persona.NombreCompleto))
                .ForMember(d => d.Especialidad, m => m.MapFrom(o => o.Especialidad.Nombre))
                .ForMember(x => x.PuedoAtender, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Atender)))
                .ForMember(x => x.PuedoCancelar, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Cancelar)))
                .ForMember(x => x.PuedoPresentar, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Presentar)));

            CreateMap<Turno, TurnosPorPacienteViewModel.HistorialItem>()
                .ForMember(x => x.PuedoAtender, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Atender)))
                .ForMember(x => x.PuedoCancelar, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Cancelar)))
                .ForMember(x => x.PuedoPresentar, m => m.ResolveUsing(o => o.PuedeAplicar(EventoTurno.Presentar)));

            #endregion
        }
    }
}