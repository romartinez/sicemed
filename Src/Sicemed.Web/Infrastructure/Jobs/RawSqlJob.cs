using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using log4net;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public abstract class RawSqlJob : SimpleJob
    {
        private ILog _log = LogManager.GetLogger(typeof (RawSqlJob));

        protected RawSqlJob(string name, TimeSpan interval, TimeSpan timeout) : base(name, interval, timeout)
        {
        }

        protected RawSqlJob(string name, TimeSpan interval) : base(name, interval)
        {
        }

        protected override void Run()
        {                        
            var connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        Run(conn, tx);
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        _log.Error(ex);
                    }
                }
            }
        }

        protected abstract void Run(SqlConnection conn, IDbTransaction tx);
    }
}