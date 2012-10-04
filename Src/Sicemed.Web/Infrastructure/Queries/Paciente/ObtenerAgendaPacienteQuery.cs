using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Transform;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Paciente;

namespace Sicemed.Web.Infrastructure.Queries.Paciente
{
    public interface IObtenerAgendaPacienteQuery : IQuery<AgendaPacienteViewModel>
    {
        long PacienteId { get; set; }
    }

    public class ObtenerAgendaPacienteQuery : Query<AgendaPacienteViewModel>, IObtenerAgendaPacienteQuery
    {
        public virtual long PacienteId { get; set; }

        protected override AgendaPacienteViewModel CoreExecute()
        {
            var desde = DateTime.Now.ToMidnigth();

            var session = SessionFactory.GetCurrentSession();

            var turnos = session.QueryOver<Turno>()
                .Fetch(t => t.Consultorio).Eager
                .Fetch(t => t.Profesional).Eager
                .Fetch(t => t.Profesional.Persona).Eager
                .Fetch(t => t.Paciente).Eager
                .Fetch(t => t.Paciente.Persona).Eager
                .Where(t => t.FechaTurno >= desde)
                .OrderBy(t => t.FechaTurno).Asc
                .JoinQueryOver(t => t.Paciente)
                .Where(p => p.Id == PacienteId)
                .TransformUsing(Transformers.DistinctRootEntity)
                .List();

            var viewModel = new AgendaPacienteViewModel { FechaTurnos = desde };
            if (turnos != null && turnos.Any())
            {
                viewModel = MappingEngine.Map<AgendaPacienteViewModel>(turnos.First().Paciente);
                viewModel.FechaTurnos = desde; //Me lo pisa el mapper
                viewModel.Turnos = MappingEngine.Map<List<AgendaPacienteViewModel.TurnoViewModel>>(turnos);
            }

            return viewModel;
        }
    }
}