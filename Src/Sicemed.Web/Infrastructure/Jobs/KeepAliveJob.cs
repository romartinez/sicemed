using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Sicemed.Web.App_Start;
using WebBackgrounder;
using log4net;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public class KeepAliveJob : Job
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(KeepAliveJob));

        public KeepAliveJob()
            : base("Keep Alive", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(30))
        {}

        public override Task Execute()
        {
            return new Task(Run);
        }

        private static void Run()
        {
            if(Log.IsInfoEnabled) Log.Info("Running KeepAlive...");
            var url = ConfigurationManager.AppSettings["KeepAliveUrl"];
            if(string.IsNullOrWhiteSpace(url)) return;

            if (Log.IsDebugEnabled) Log.DebugFormat("Pinging {0}", url);

            var client = new WebClient();
            client.DownloadStringAsync(new Uri(url));
        }
    }
}