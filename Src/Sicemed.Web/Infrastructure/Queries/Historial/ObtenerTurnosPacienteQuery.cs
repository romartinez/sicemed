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

    public class ObtenerTurnosPacienteQuery : Query<TurnosPacienteViewModel>, IObtenerTurnosPacienteQuery
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
                .Where(t => t.Paciente == paciente)
                .OrderBy(t => t.FechaTurno).Desc;

            if (!string.IsNullOrWhiteSpace(Filtro))
            {
                Models.Roles.Profesional profesional = null;
                Persona persona = null;

                query.Left.JoinAlias(x => x.Profesional, ()=> profesional)
                    .Left.JoinAlias(x => profesional.Persona, () => persona)
                .Where(
                    Restrictions.On<Turno>(x => x.Nota).IsInsensitiveLike(Filtro, MatchMode.Anywhere)
                        || Restrictions.On(() => persona.Nombre).IsInsensitiveLike(Filtro, MatchMode.Start)
                        || Restrictions.On(() => persona.SegundoNombre).IsInsensitiveLike(Filtro, MatchMode.Start)
                        || Restrictions.On(() => persona.Apellido).IsInsensitiveLike(Filtro, MatchMode.Start)
                );
            }

            var turnos = query.Future();
            var hc = new TurnosPacienteViewModel();

            hc.Turnos = MappingEngine.Map<IEnumerable<TurnosPacienteViewModel.HistorialItem>>(turnos);

            return hc;
        }
    }
}