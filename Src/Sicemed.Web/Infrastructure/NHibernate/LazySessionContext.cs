using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;

namespace Sicemed.Web.Infrastructure.NHibernate
{
    public class LazySessionContext : CurrentSessionContext
    {
        private readonly ISessionFactoryImplementor _factory;
        private const string CURRENT_SESSION_CONTEXT_KEY = "NHibernateCurrentSession";

        public LazySessionContext(ISessionFactoryImplementor factory)
        {
            this._factory = factory;
        }

        protected override ISession Session
        {
            get
            {
                Lazy<ISession> initializer;
                var currentSessionFactoryMap = GetCurrentFactoryMap();
                if (currentSessionFactoryMap == null ||
                    !currentSessionFactoryMap.TryGetValue(_factory, out initializer))
                {
                    return null;
                }
                return initializer.Value;
            }
            set
            {
                Lazy<ISession> initializer;
                var currentSessionFactoryMap = GetCurrentFactoryMap();
                if (currentSessionFactoryMap == null ||
                    !currentSessionFactoryMap.TryGetValue(_factory, out initializer))
                {
                    currentSessionFactoryMap.Add(_factory, new Lazy<ISession>(()=> value));
                }
                currentSessionFactoryMap[_factory] = new Lazy<ISession>(() => value);
            }
        }

        /// <summary>
        /// Bind a new sessionInitializer to the context of the sessionFactory.
        /// </summary>
        /// <param name="sessionInitializer"></param>
        /// <param name="sessionFactory"></param>
        public static void Bind(Lazy<ISession> sessionInitializer, ISessionFactory sessionFactory)
        {
            var map = GetCurrentFactoryMap();
            map[sessionFactory] = sessionInitializer;
        }

        /// <summary>
        /// Unbind the current session of the session factory.
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <returns></returns>
        public static ISession UnBind(ISessionFactory sessionFactory)
        {
            var map = GetCurrentFactoryMap();
            var sessionInitializer = map[sessionFactory];
            map[sessionFactory] = null;
            if (sessionInitializer == null || !sessionInitializer.IsValueCreated) return null;
            return sessionInitializer.Value;
        }

        /// <summary>
        /// Provides the CurrentMap of SessionFactories.
        /// If there is no map create/store and return a new one.
        /// </summary>
        /// <returns></returns>
        private static IDictionary<ISessionFactory, Lazy<ISession>> GetCurrentFactoryMap()
        {
            var currentFactoryMap = (IDictionary<ISessionFactory, Lazy<ISession>>)
                                    HttpContext.Current.Items[CURRENT_SESSION_CONTEXT_KEY];
            if (currentFactoryMap == null)
            {
                currentFactoryMap = new Dictionary<ISessionFactory, Lazy<ISession>>();
                HttpContext.Current.Items[CURRENT_SESSION_CONTEXT_KEY] = currentFactoryMap;
            }
            return currentFactoryMap;
        }
    }
}