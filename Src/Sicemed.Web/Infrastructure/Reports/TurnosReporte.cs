using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Transform;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Reports;

namespace Sicemed.Web.Infrastructure.Reports
{
    public interface ITurnosReporte : IReport<TurnosReportModel>
    {
        DateTime Fecha { get; set; }
    }

    public class TurnosReporte : ReporteBase<TurnosReportModel>, ITurnosReporte
    {
        public virtual DateTime Fecha { get; set; }

        public override string Title { get { return string.Format("Turnos Del Día: {0:dd/MM/yyyy}", Fecha); } }

        public override string Name { get { return "TurnosReporte"; } }        

        public override IEnumerable<TurnosReportModel> Execute()
        {
            var desde = Fecha.ToMidnigth();
            var hasta = desde.AddDays(1).ToMidnigth();

            var session = SessionFactory.GetCurrentSession();

            var turnos = session.QueryOver<Turno>()
                .Fetch(t => t.Consultorio).Eager
                .Fetch(t => t.Profesional).Eager
                .Fetch(t => t.Profesional.Persona).Eager
                .Fetch(t => t.Paciente).Eager
                .Fetch(t => t.Paciente.Persona).Eager
                .Where(t => t.Estado != EstadoTurno.Cancelado)
                .Where(t => t.FechaTurno >= desde)
                .And(t => t.FechaTurno <= hasta)
                .TransformUsing(Transformers.DistinctRootEntity)                
                .OrderBy(t => t.FechaTurno).Asc
                .List();

            return turnos.Select(Convert);
        }

        private TurnosReportModel Convert(Turno turno)
        {
            return new TurnosReportModel
            {
                FechaTurnos = turno.FechaTurno.ToMidnigth(),
                Profesional = turno.Profesional.Persona.NombreCompleto,
                Paciente = turno.Paciente.Persona.NombreCompleto,
                Consultorio = turno.Consultorio != null ? turno.Consultorio.Nombre : string.Empty,
                Especialidad = turno.Especialidad != null ? turno.Especialidad.Nombre : string.Empty,
                FechaTurno = turno.FechaTurno,
                FechaTurnoFin = turno.FechaTurnoFinal
            };
        }
    }
}