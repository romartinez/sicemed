using System;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Sicemed.Web.Infrastructure.Queries.Jobs;
using WebBackgrounder;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public class AusentarTurnosVencidosJob : Job
    {
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
            var turnos = ServiceLocator.Current.GetInstance<IObtenerTurnosAMarcarComoAusentadosQuery>().Execute();

            foreach (var turno in turnos)
            {
                turno.MarcarAusente();
            }
        }
    }
}