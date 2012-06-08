using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Profesional;

namespace Sicemed.Web.Infrastructure.Queries.Profesional
{
    public interface IObtenerAgendaProfesionalQuery : IQuery<AgendaProfesionalViewModel>
    {
        long ProfesionalId { get; set; }
        DateTime? Fecha { get; set; }
    }

    public class ObtenerAgendaProfesionalQuery : Query<AgendaProfesionalViewModel>, IObtenerAgendaProfesionalQuery
    {
        public virtual long ProfesionalId { get; set; }
        public virtual DateTime? Fecha { get; set; }

        protected override AgendaProfesionalViewModel CoreExecute()
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
                .OrderBy(t => t.FechaTurno).Asc
                .JoinQueryOver(t => t.Profesional)
                .Where(p => p.Id == ProfesionalId)
                .List();

            var viewModel = new AgendaProfesionalViewModel { FechaTurnos = desde };
            if (turnos != null && turnos.Any())
            {
                viewModel = MappingEngine.Map<AgendaProfesionalViewModel>(turnos.First().Profesional);
                viewModel.FechaTurnos = desde; //Me lo pisa el mapper
                viewModel.Turnos = MappingEngine.Map<List<AgendaProfesionalViewModel.TurnoViewModel>>(turnos);
            }

            return viewModel;
        }
    }
}