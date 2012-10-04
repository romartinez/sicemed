using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Transform;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Secretaria;

namespace Sicemed.Web.Infrastructure.Queries.Secretaria
{
    public interface IObtenerTurnosPorFechaQuery : IQuery<TurnosDelDiaViewModel>
    {
        DateTime? Fecha { get; set; }
    }

    public class ObtenerTurnosPorFechaQuery : Query<TurnosDelDiaViewModel>, IObtenerTurnosPorFechaQuery
    {
        public virtual DateTime? Fecha { get; set; }

        protected override TurnosDelDiaViewModel CoreExecute()
        {
            var desde = Fecha ?? DateTime.Now;
            desde = desde.ToMidnigth();
            var hasta = desde.AddDays(1).ToMidnigth();

            var session = SessionFactory.GetCurrentSession();

            var turnos = session.QueryOver<Turno>()
                .Fetch(t => t.Consultorio).Eager
                .Fetch(t => t.Profesional).Eager
                .Fetch(t => t.Profesional.Persona).Eager
                .Fetch(t => t.Paciente).Eager
                .Fetch(t => t.Paciente.Persona).Eager
                .Where(t => t.FechaTurno >= desde)
                .And(t => t.FechaTurno <= hasta)
                .TransformUsing(Transformers.DistinctRootEntity)
                .OrderBy(t => t.FechaTurno).Asc
                .List();

            var viewModel = new TurnosDelDiaViewModel { FechaTurnos = desde };
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