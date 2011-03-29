using System;
using System.Web;
using NHibernate;

namespace Sicemed.Web.Plumbing.HttpModules
{
    public class NHibernateSessionModule : IHttpModule
    {
        private HttpApplication _httpApplication;
        private ISessionFactoryProvider _sessionFactoryProvider;

        public void Init(HttpApplication context)
        {
            _httpApplication = context;
            _sessionFactoryProvider = (ISessionFactoryProvider)
                      context.Application[SessionFactoryProvider.Key];
            context.BeginRequest += ContextBeginRequest;
            context.EndRequest += ContextEndRequest;
        }

        private void ContextBeginRequest(object sender, EventArgs e)
        {
            foreach (var sessionFactory in _sessionFactoryProvider.GetSessionFactories())
            {
                var localFactory = sessionFactory;
                LazySessionContext.Bind(
                    new Lazy<ISession>(() => BeginSession(localFactory)),
                    sessionFactory);
            }
        }

        private static ISession BeginSession(ISessionFactory sf)
        {
            var session = sf.OpenSession();
            session.BeginTransaction();
            return session;
        }

        private void ContextEndRequest(object sender, EventArgs e)
        {
            foreach (var sessionFactory in _sessionFactoryProvider.GetSessionFactories())
            {
                var session = LazySessionContext.UnBind(sessionFactory);
                if (session == null) continue;
                EndSession(session);
            }
        }

        private static void EndSession(ISession session)
        {
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Commit();
            }
            session.Dispose();
        }

        public void Dispose()
        {
            _httpApplication.BeginRequest -= ContextBeginRequest;
            _httpApplication.EndRequest -= ContextEndRequest;
        }
    }
}