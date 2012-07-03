using System;
using System.Configuration;
using System.Net;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public class KeepAliveJob : SimpleJob
    {
        public KeepAliveJob()
            : base("Keep Alive", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(30))
        {}

        protected override void Run()
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