using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Secretaria;

namespace Sicemed.Web.Infrastructure.Queries.Secretaria
{
    public interface IObtenerTurnosPorFechaQuery : IQuery<TurnosDelDiaViewModel>
    {
        DateTime? Desde { get; set; }
        DateTime? Hasta { get; set; }
    }

    public class ObtenerTurnosPorFechaQuery : Query<TurnosDelDiaViewModel>, IObtenerTurnosPorFechaQuery
    {
        public virtual DateTime? Desde { get; set; }
        public virtual DateTime? Hasta { get; set; }

        protected override TurnosDelDiaViewModel CoreExecute()
        {
            var dateDesde = Desde ?? DateTime.Now;
            dateDesde = dateDesde.ToMidnigth();
            var dateHasta = Hasta ?? DateTime.Now.AddDays(1);
            dateHasta = dateHasta.ToMidnigth();

            var session = SessionFactory.GetCurrentSession();

            var turnos = session.QueryOver<Turno>()
                .Fetch(t => t.Consultorio).Eager
                .Fetch(t => t.Profesional).Eager
                .Fetch(t => t.Profesional.Persona).Eager
                .Fetch(t => t.Paciente).Eager
                .Fetch(t => t.Paciente.Persona).Eager
                .Where(t => t.FechaTurno >= dateDesde)
                .And(t => t.FechaTurno <= dateHasta)
                .OrderBy(t => t.FechaTurno).Asc
                .List();

            var viewModel = new TurnosDelDiaViewModel();
            if (turnos != null)
            {
                viewModel.ProfesionalesConTurnos = turnos.Select(t => t.Profesional)
                    .Distinct()
                    .Select(p => MappingEngine.Map<TurnosDelDiaViewModel.ProfesionalViewModel>(p))
                    .OrderBy(p => p.PersonaNombreCompleto)
                    .ToList();
                foreach (var profesional in viewModel.ProfesionalesConTurnos)
                {
                    var turnosProfesional = turnos.Where(t => t.Profesional.Id == profesional.Id);
                    profesional.Turnos = MappingEngine.Map<List<TurnosDelDiaViewModel.TurnoViewModel>>(turnosProfesional);
                }
            }

            return viewModel;
        }
    }
}