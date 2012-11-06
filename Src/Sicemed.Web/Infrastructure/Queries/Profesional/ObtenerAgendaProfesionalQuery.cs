using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Transform;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
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
                .TransformUsing(Transformers.DistinctRootEntity)
                .List();

            var viewModel = new AgendaProfesionalViewModel { FechaTurnos = desde };
            if (turnos != null && turnos.Any())
            {
                viewModel = MappingEngine.Map<AgendaProfesionalViewModel>(turnos.First().Profesional);
                viewModel.FechaTurnos = desde; //Me lo pisa el mapper

                //Calculo los datos del paciente
                foreach (var turno in turnos)
                {
                    var turnoViewModel = MappingEngine.Map<AgendaProfesionalViewModel.TurnoViewModel>(turno);
                    turnoViewModel.Paciente = CrearPaciente(turno);
                    viewModel.Turnos.Add(turnoViewModel);
                }
            }

            return viewModel;
        }

        private AgendaProfesionalViewModel.PacienteViewModel CrearPaciente(Turno turno)
        {
            var session = SessionFactory.GetCurrentSession();
            var paciente = new AgendaProfesionalViewModel.PacienteViewModel();
            paciente.Descripcion = turno.Paciente.Persona.NombreCompleto;
            paciente.Id = turno.Paciente.Id;
            paciente.NumeroAfiliado = turno.Paciente.NumeroAfiliado;
            paciente.Edad = turno.Paciente.Persona.Edad;
            paciente.Altura = turno.Paciente.Persona.Peso;
            paciente.Peso = turno.Paciente.Persona.Altura;
            if (turno.Paciente.Plan != null)
            {
                paciente.Plan = turno.Paciente.Plan.Nombre;
                paciente.ObraSocial = turno.Paciente.Plan.ObraSocial.RazonSocial;
            }

            //NOTE: Ver de armar otra query para calcular si es la primera vez.
            //Es la primera vez si hay un turno en estado atendido.
            var turnoAtendido = session.QueryOver<Turno>().Where(x => x.Paciente == turno.Paciente)
                    .And(x => x.Profesional == turno.Profesional)
                    .And(x => x.Especialidad == turno.Especialidad)
                    .And(x => x.Estado == EstadoTurno.Atendido).Take(1).SingleOrDefault();
            
            paciente.EsPrimeraVez = turnoAtendido == default(Turno);

            return paciente;
        }
    }
}
