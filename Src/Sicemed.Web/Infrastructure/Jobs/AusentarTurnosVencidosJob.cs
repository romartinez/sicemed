using System;
using NHibernate;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public class AusentarTurnosVencidosJob : NHibernateJob
    {
        public AusentarTurnosVencidosJob()
            : base("Ausentar Turnos Vencidos", TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(30))
        {
        }

        protected override void Run(ISession session)
        {
            if (Log.IsInfoEnabled) Log.Info("Running AusentarTurnosVencidosJob");
            
            var turnos = session.QueryOver<Turno>()
                .Where(t => t.Estado == EstadoTurno.Otorgado
                    && t.FechaTurno <= DateTime.Now.AddDays(-1))
                .List();

            foreach (var turno in turnos)
            {
                turno.MarcarAusente();
            }
            
            if (Log.IsInfoEnabled) Log.InfoFormat("Marcados como ausentes {0} turnos.", turnos.Count);
        }
    }
}