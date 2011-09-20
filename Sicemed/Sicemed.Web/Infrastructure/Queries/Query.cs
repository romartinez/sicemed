using System.Collections.Generic;
using System.Diagnostics;
using Castle.Core.Logging;
using NHibernate;
using Newtonsoft.Json;

namespace Sicemed.Web.Infrastructure.Queries
{
    public abstract class Query<T> : IQuery<T>
    {
        private const long QUERY_THREADSHOLD = 100; //milliseconds
        [JsonIgnore]
        public virtual ISessionFactory SessionFactory { get; set; }
        [JsonIgnore]
        public virtual ILogger Logger { get; set; }

        #region Implementation of IQuery<T>

        public abstract IEnumerable<T> CoreExecute();

        public virtual IEnumerable<T> Execute()
        {
            if (Logger.IsInfoEnabled) Logger.InfoFormat("Ejecutando query con parametros:\n {1}", JsonConvert.SerializeObject(this, Formatting.Indented));
            var watcher = Stopwatch.StartNew();
            var result = CoreExecute();
            
            watcher.Stop();
            
            if (Logger.IsDebugEnabled || (watcher.ElapsedMilliseconds >= QUERY_THREADSHOLD && Logger.IsWarnEnabled))
                Logger.WarnFormat("El query demoró: {0}ms", watcher.ElapsedMilliseconds);

            if (Logger.IsInfoEnabled) Logger.InfoFormat("Resultado del query:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            return result;
        }

        #endregion
    }
}