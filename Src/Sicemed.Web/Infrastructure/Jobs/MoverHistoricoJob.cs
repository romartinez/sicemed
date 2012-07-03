using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Sicemed.Web.Infrastructure.Helpers;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public class MoverHistoricoJob : RawSqlJob
    {
        public MoverHistoricoJob()
            : base("MoverHistoricoJob", TimeSpan.FromDays(1), TimeSpan.FromMinutes(5))
        {

        }

        protected override void Run(SqlConnection conn, IDbTransaction tx)
        {
            Log.Info("Moviendo a Historico");

            var diasLogAMantener = Convert.ToInt64(ConfigurationManager.AppSettings["DiasLogAMantener"]);
            var diasAuditoriaAMantener = Convert.ToInt64(ConfigurationManager.AppSettings["DiasAuditoriaAMantener"]);

            var fechaLog = DateTime.Now.ToMidnigth().Subtract(TimeSpan.FromDays(diasLogAMantener));
            var fechaAuditoria = DateTime.Now.ToMidnigth().Subtract(TimeSpan.FromDays(diasAuditoriaAMantener));

            var sql = "INSERT INTO [AuditLogHistorico] SELECT * FROM [AuditLog] WHERE [Fecha] < @FechaAuditoria " +
                      "INSERT INTO [LogHistorico] SELECT * FROM  [Log] WHERE [Date] < @FechaLog " +
                      "DELETE [AuditLog] WHERE [Fecha] < @FechaAuditoria " +
                      "DELETE [Log] WHERE [Date] < @FechaLog ";
            
            conn.Execute(sql, new {FechaLog = fechaLog, FechaAuditoria = fechaAuditoria}, tx);
        }
    }
}