using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Infrastructure.Queries.Profesional;
using Sicemed.Web.Models.Reports;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.Reports
{
    public interface ITurnosDelDiaReporte : IReport<TurnosDelDiaReportModel>
    { }

    public class TurnosDelDiaReporte : ReporteBase<TurnosDelDiaReportModel>, ITurnosDelDiaReporte
    {
        public override IEnumerable<TurnosDelDiaReportModel> Execute()
        {
            var query = QueryFactory.Create<IObtenerAgendaProfesionalQuery>();
            query.ProfesionalId = User.As<Profesional>().Id;
            query.Fecha = DateTime.Now;
            var turnos = query.Execute();
            return turnos.Turnos.Select(x => new TurnosDelDiaReportModel
                {
                    NombrePaciente = x.Paciente.Descripcion,
                    FechaFinTurno = x.FechaTurnoFinal,
                    FechaInicioTurno = x.FechaTurno,
                    NombreProfesional = User.NombreCompleto
                });
        }
    }
}