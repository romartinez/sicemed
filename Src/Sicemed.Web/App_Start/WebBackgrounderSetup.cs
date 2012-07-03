using Sicemed.Web.Infrastructure.Jobs;
using WebBackgrounder;
using log4net;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Sicemed.Web.App_Start.WebBackgrounderSetup), "Start")]
[assembly: WebActivator.ApplicationShutdownMethod(typeof(Sicemed.Web.App_Start.WebBackgrounderSetup), "Shutdown")]

namespace Sicemed.Web.App_Start
{
    public static class WebBackgrounderSetup
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebBackgrounderSetup));
        private static readonly JobHost JobHost = new JobHost();
        private static readonly JobManager JobManager = CreateJobWorkersManager();

        public static void Start()
        {

            JobManager.Start();
        }

        public static void Shutdown()
        {
            JobManager.Dispose();
        }

        private static JobManager CreateJobWorkersManager()
        {
            var jobs = new IJob[]
            {
                new KeepAliveJob(),                 
                new AusentarTurnosVencidosJob(), 
                new MoverHistoricoJob(), 
            };
            
            var manager = new JobManager(jobs, JobHost);
            manager.Fail(ex => Log.Fatal(ex));
            return manager;
        }
    }
}