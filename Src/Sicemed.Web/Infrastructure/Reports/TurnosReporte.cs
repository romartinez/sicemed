using System;
using System.Collections.Generic;
using Sicemed.Web.Infrastructure.Queries.Secretaria;
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

        public override IEnumerable<TurnosReportModel> Execute()
        {
            var query = QueryFactory.Create<ITurnosReporteQuery>();            
            query.Fecha = Fecha;
            return query.Execute();
        }
    }
}