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
        DateTime? Desde { get; set; }
        DateTime? Hasta { get; set; }
    }

    public class ObtenerAgendaProfesionalQuery : Query<AgendaProfesionalViewModel>, IObtenerAgendaProfesionalQuery
    {
        public virtual long ProfesionalId { get; set; }
        public virtual DateTime? Desde { get; set; }
        public virtual DateTime? Hasta { get; set; }

        protected override AgendaProfesionalViewModel CoreExecute()
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
                .JoinQueryOver(t => t.Profesional)
                .Where(p => p.Id == ProfesionalId)
                .List();

            var viewModel = new AgendaProfesionalViewModel();
            if (turnos != null && turnos.Any())
            {
                viewModel = MappingEngine.Map<AgendaProfesionalViewModel>(turnos.First().Profesional);
                viewModel.Turnos = MappingEngine.Map<List<AgendaProfesionalViewModel.TurnoViewModel>>(turnos);
            }

            return viewModel;
        }
    }
}