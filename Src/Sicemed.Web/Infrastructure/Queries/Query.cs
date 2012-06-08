using System.Diagnostics;
using AutoMapper;
using Castle.Core.Logging;
using NHibernate;
using Newtonsoft.Json;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Providers.Cache;

namespace Sicemed.Web.Infrastructure.Queries
{
    public abstract class Query<T> : IQuery<T>
    {
        private const long QUERY_THREADSHOLD = 100; //milliseconds
        [JsonIgnore]
        public virtual ISessionFactory SessionFactory { get; set; }
        [JsonIgnore]
        public virtual ILogger Logger { get; set; }
        [JsonIgnore]
        public virtual IMappingEngine MappingEngine { get; set; }
        [JsonIgnore]
        public virtual ICacheProvider Cache { get; set; }
        [JsonIgnore]
        public virtual IQueryFactory QueryFactory { get; set; }

        protected Query()
        {
            Logger = NullLogger.Instance;
        }

        #region Implementation of IQuery<T>

        protected abstract T CoreExecute();

        public virtual T Execute()
        {
            if (Logger.IsInfoEnabled) Logger.InfoFormat("Ejecutando query con parametros:\n {0}", Json.SerializeObject(this));
            var watcher = Stopwatch.StartNew();
            var result = CoreExecute();
            
            watcher.Stop();
            
            if (Logger.IsDebugEnabled || (watcher.ElapsedMilliseconds >= QUERY_THREADSHOLD && Logger.IsWarnEnabled))
                Logger.WarnFormat("El query demoró: {0}ms", watcher.ElapsedMilliseconds);

            if (Logger.IsInfoEnabled) Logger.InfoFormat("Resultado del query:\n {0}", Json.SerializeObject(result));
            return result;
        }

        public virtual void ClearCache()
        {
            //Do nothing...
        }
        #endregion
    }
}