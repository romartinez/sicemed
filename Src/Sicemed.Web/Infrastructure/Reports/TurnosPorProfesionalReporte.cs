using System;
using System.Linq;
using System.Collections.Generic;
using Sicemed.Web.Infrastructure.Queries.Profesional;
using Sicemed.Web.Models.Reports;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.Reports
{
    public interface ITurnosPorProfesionalReporte : IReport<TurnosPorProfesionalReportModel>
    {}

    public class TurnosPorProfesionalReporte : ReporteBase<TurnosPorProfesionalReportModel>, ITurnosPorProfesionalReporte
    {
        public override IEnumerable<TurnosPorProfesionalReportModel> Execute()
        {
            var query = QueryFactory.Create<IObtenerAgendaProfesionalQuery>();
            query.ProfesionalId = User.As<Profesional>().Id;
            query.Fecha = DateTime.Now;
            var turnos = query.Execute();
            return turnos.Turnos.Select(x => new TurnosPorProfesionalReportModel
                                                 {
                                                    FechaTurno = x.FechaTurno,
                                                    Nombre = User.NombreCompleto,
                                                    Paciente = x.Paciente.Descripcion
                                                 });
        }
    }
}