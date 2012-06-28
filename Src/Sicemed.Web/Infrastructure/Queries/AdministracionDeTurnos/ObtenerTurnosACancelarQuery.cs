using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Profesional;

namespace Sicemed.Web.Infrastructure.Queries.AdministracionDeTurnos
{
    public interface IObtenerTurnosACancelarQuery : IQuery<IEnumerable<Turno>>
    {
        long ProfesionalId { get; set; }
        DateTime Fecha { get; set; }
    }

    public class ObtenerTurnosACancelarQuery : Query<IEnumerable<Turno>>, IObtenerTurnosACancelarQuery
    {
        public virtual long ProfesionalId { get; set; }
        public virtual DateTime Fecha { get; set; }

        protected override IEnumerable<Turno> CoreExecute()
        {
            var desde = Fecha.ToMidnigth();
            var hasta = desde.AddDays(1);

            var session = SessionFactory.GetCurrentSession();

            var turnos = session.QueryOver<Turno>()
                .Fetch(t => t.Consultorio).Eager
                .Fetch(t => t.Profesional).Eager
                .Fetch(t => t.Profesional.Persona).Eager
                .Fetch(t => t.Paciente).Eager
                .Fetch(t => t.Paciente.Persona).Eager
                .Where(t => t.FechaTurno >= desde)
                .And(t => t.FechaTurno <= hasta)
                .AndRestrictionOn(t => t.Estado)
                .IsIn(Turno.EstadosAplicaEvento(Turno.EventoTurno.Cancelar))
                .OrderBy(t => t.FechaTurno).Asc
                .JoinQueryOver(t => t.Profesional)
                .Where(p => p.Id == ProfesionalId)
                .List();
            
            return turnos;
        }
    }
}