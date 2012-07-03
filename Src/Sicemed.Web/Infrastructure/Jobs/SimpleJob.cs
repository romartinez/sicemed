using System;
using System.Reflection;
using System.Threading.Tasks;
using WebBackgrounder;
using log4net;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public abstract class SimpleJob : Job
    {
        protected readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected SimpleJob(string name, TimeSpan interval, TimeSpan timeout) : base(name, interval, timeout)
        {
        }

        protected SimpleJob(string name, TimeSpan interval) : base(name, interval)
        {
        }

        protected abstract void Run();

        public override Task Execute()
        {
            if(Log.IsInfoEnabled) Log.InfoFormat("Executing...");
            return new Task(Run);
        }
    }
}