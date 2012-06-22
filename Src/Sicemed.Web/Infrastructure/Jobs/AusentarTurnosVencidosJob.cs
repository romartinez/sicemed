using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Sicemed.Web.Infrastructure.Queries.Jobs;
using WebBackgrounder;
using log4net;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public class AusentarTurnosVencidosJob : Job
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AusentarTurnosVencidosJob));

        public AusentarTurnosVencidosJob() 
            : base("Ausentar Turnos Vencidos", TimeSpan.FromDays(1), TimeSpan.FromMinutes(5))
        {
        }

        public override Task Execute()
        {
            return new Task(Run);
        }

        private static void Run()
        {
            if (Log.IsInfoEnabled) Log.Info("Running AusentarTurnosVencidosJob");
            var turnos = ServiceLocator.Current.GetInstance<IObtenerTurnosAMarcarComoAusentadosQuery>().Execute().ToList();

            foreach (var turno in turnos)
            {
                turno.MarcarAusente();
            }

            if (Log.IsDebugEnabled) Log.DebugFormat("Marcados como ausentes {0} turnos.", turnos.Count);
        }
    }
}