using System;
using System.Collections.Generic;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.HistorialAtenciones;

namespace Sicemed.Web.Infrastructure.Queries.HistorialAtenciones
{
    public interface IObtenerHistorialAtencionesQuery : IQuery<HistorialAtencionesViewModel>
    {
        DateTime FechaHasta { get; set; }
        DateTime FechaDesde { get; set; }
        long PacienteId { get; set; }
        long? ProfesionalId { get; set; }
    }

    public class ObtenerHistorialAtencionesQuery : Query<HistorialAtencionesViewModel>, IObtenerHistorialAtencionesQuery
    {
        public DateTime FechaHasta { get; set; }
        public DateTime FechaDesde { get; set; }
        
        public long PacienteId { get; set; }
        public long? ProfesionalId { get; set; }

        protected override HistorialAtencionesViewModel CoreExecute()
        {
            var session = SessionFactory.GetCurrentSession();
            var paciente = session.Load<Models.Roles.Paciente>(PacienteId);
            var query = session.QueryOver<Turno>()
                .Fetch(t => t.Paciente).Eager
                .Fetch(t => t.Paciente.Persona).Eager
                .Fetch(t => t.Profesional).Eager
                .Fetch(t => t.Profesional.Persona).Eager
                .Fetch(t => t.Consultorio).Eager
                .Fetch(t => t.Especialidad).Eager
                .Where(t=>t.FechaTurno >= FechaDesde)
                .Where(t=>t.FechaTurno <= FechaHasta)
                .OrderBy(t=>t.FechaTurno).Desc
                .Where(t => t.Paciente == paciente)
                .Where(t => t.FechaAtencion != null);

            if (ProfesionalId.HasValue) query = query.Where(t => t.Profesional == session.Load<Models.Roles.Profesional>(ProfesionalId));

            var turnos = query.Future();
            var hc = new HistorialAtencionesViewModel
                         {
                             Paciente = paciente.Persona.NombreCompleto
                         };

            hc.Turnos = MappingEngine.Map<IEnumerable<HistorialAtencionesViewModel.HistorialItem>>(turnos);

            return hc;
        }
    }
}