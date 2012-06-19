using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Historial;

namespace Sicemed.Web.Infrastructure.Queries.Historial
{

    public interface IObtenerTurnosPacienteQuery : IQuery<TurnosPacienteViewModel>
    {
        DateTime FechaHasta { get; set; }
        DateTime FechaDesde { get; set; }
        string Filtro { get; set; }
        long PacienteId { get; set; }
    }

    public class ObtenerTurnosPacientePacienteQuery : Query<TurnosPacienteViewModel>, IObtenerTurnosPacienteQuery
    {
        public DateTime FechaHasta { get; set; }
        public DateTime FechaDesde { get; set; }
        public string Filtro { get; set; }
        public long PacienteId { get; set; }

        protected override TurnosPacienteViewModel CoreExecute()
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
                .Where(t => t.FechaTurno >= FechaDesde)
                .Where(t => t.FechaTurno <= FechaHasta)
                .OrderBy(t => t.FechaTurno).Desc
                .Where(t => t.Paciente == paciente);

            if (!string.IsNullOrWhiteSpace(Filtro))
            {
                query = query.Where(Restrictions.InsensitiveLike("Nota", Filtro, MatchMode.Anywhere)
                    || Restrictions.InsensitiveLike("Profesional.Persona.Nombre", Filtro, MatchMode.Anywhere)
                    || Restrictions.InsensitiveLike("Profesional.Persona.Apellido", Filtro, MatchMode.Anywhere)
                    || Restrictions.InsensitiveLike("Profesional.Persona.SegundoNombre", Filtro, MatchMode.Anywhere));
            }

            var turnos = query.Future();
            var hc = new TurnosPacienteViewModel();

            hc.Turnos = MappingEngine.Map<IEnumerable<TurnosPacienteViewModel.HistorialItem>>(turnos);

            return hc;
        }
    }
}