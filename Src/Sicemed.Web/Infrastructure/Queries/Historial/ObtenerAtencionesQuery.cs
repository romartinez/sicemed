using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Historial;

namespace Sicemed.Web.Infrastructure.Queries.Historial
{

    public interface IObtenerAtencionesQuery : IQuery<IEnumerable<AtencionesViewModel.HistorialItem>>
    {
        DateTime FechaHasta { get; set; }
        DateTime FechaDesde { get; set; }
        string Filtro { get; set; }
        long PacienteId { get; set; }
        long ProfesionalId { get; set; }
    }

    public class ObtenerAtencionesQuery : Query<IEnumerable<AtencionesViewModel.HistorialItem>>, IObtenerAtencionesQuery
    {
        public DateTime FechaHasta { get; set; }
        public DateTime FechaDesde { get; set; }

        public string Filtro { get; set; }

        public long PacienteId { get; set; }
        public long ProfesionalId { get; set; }

        protected override IEnumerable<AtencionesViewModel.HistorialItem> CoreExecute()
        {
            var session = SessionFactory.GetCurrentSession();
            var paciente = session.Load<Models.Roles.Paciente>(PacienteId);
            var profesional = session.Load<Models.Roles.Profesional>(ProfesionalId);
            var query = session.QueryOver<Turno>()
                .Fetch(t => t.Paciente).Eager
                .Fetch(t => t.Paciente.Persona).Eager
                .Fetch(t => t.Profesional).Eager
                .Fetch(t => t.Profesional.Persona).Eager
                .Fetch(t => t.Consultorio).Eager
                .Fetch(t => t.Especialidad).Eager
                .Where(t => t.FechaTurno >= FechaDesde)
                .Where(t => t.FechaTurno <= FechaHasta)
                .OrderBy(t => t.FechaTurno).Desc
                .Where(t => t.Paciente == paciente)
                .Where(t => t.FechaAtencion != null)
                .Where(t => t.Profesional == profesional);

            if (!string.IsNullOrWhiteSpace(Filtro))
                query = query.Where(Restrictions.On<Turno>(x => x.Nota).IsInsensitiveLike(Filtro, MatchMode.Anywhere));

            var turnos = query.Future();

            return MappingEngine.Map<IEnumerable<AtencionesViewModel.HistorialItem>>(turnos);
        }
    }
}