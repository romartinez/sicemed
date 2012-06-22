using System;
using System.Threading.Tasks;
using Sicemed.Web.Models;
using log4net;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public class AusentarTurnosVencidosJob : NHibernateJob
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AusentarTurnosVencidosJob));

        public AusentarTurnosVencidosJob()
            : base("Ausentar Turnos Vencidos", TimeSpan.FromMinutes(15), TimeSpan.FromSeconds(30))
        {
        }

        public override Task Execute()
        {
            return new Task(Run);
        }

        private static void Run()
        {
            if (Log.IsInfoEnabled) Log.Info("Running AusentarTurnosVencidosJob");
            var turnos = Session.QueryOver<Turno>()
                .Where(t => t.Estado == Turno.EstadoTurno.Otorgado
                    && t.FechaTurno <= DateTime.Now.AddDays(-1))
                .List();            

            foreach (var turno in turnos)
            {
                turno.MarcarAusente();
            }

            if (Log.IsDebugEnabled) Log.DebugFormat("Marcados como ausentes {0} turnos.", turnos.Count);
        }
    }
}