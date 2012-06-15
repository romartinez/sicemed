using System;
using System.Collections.Generic;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Historial;

namespace Sicemed.Web.Infrastructure.Queries.Historial
{

    public interface IObtenerTurnosQuery : IQuery<TurnosViewModel>
    {
        DateTime FechaHasta { get; set; }
        DateTime FechaDesde { get; set; }
        long PacienteId { get; set; }
    }

    public class ObtenerTurnosQuery : Query<TurnosViewModel>, IObtenerTurnosQuery
    {
        public DateTime FechaHasta { get; set; }
        public DateTime FechaDesde { get; set; }
        
        public long PacienteId { get; set; }

        protected override TurnosViewModel CoreExecute()
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
                .Where(t => t.Paciente == paciente);

            var turnos = query.Future();
            var hc = new TurnosViewModel
                         {
                             //Paciente = paciente.Persona.NombreCompleto
                         };

            hc.Turnos = MappingEngine.Map<IEnumerable<TurnosViewModel.HistorialItem>>(turnos);

            return hc;
        }
    }
}