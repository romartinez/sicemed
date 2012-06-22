using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using WebBackgrounder;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public class KeepAliveJob : Job
    {
        public KeepAliveJob()
            : base("Keep Alive", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(30))
        {}

        public override Task Execute()
        {
            return new Task(Run);
        }

        private static void Run()
        {
            var url = ConfigurationManager.AppSettings["KeepAliveUrl"];
            if(string.IsNullOrWhiteSpace(url)) return;

            var client = new WebClient();
            client.DownloadStringAsync(new Uri(url));
        }
    }
}