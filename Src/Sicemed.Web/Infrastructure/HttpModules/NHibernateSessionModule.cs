using System;
using System.Web;
using NHibernate;
using Sicemed.Web.Infrastructure.NHibernate;

namespace Sicemed.Web.Infrastructure.HttpModules
{
    public class NHibernateSessionModule : IHttpModule
    {
        private HttpApplication app;
        private ISessionFactoryProvider sfp;

        public void Init(HttpApplication context)
        {
            app = context;
            sfp = (ISessionFactoryProvider)
                      context.Application[SessionFactoryProvider.Key];
            context.BeginRequest += ContextBeginRequest;
            context.EndRequest += ContextEndRequest;
        }

        private void ContextBeginRequest(object sender, EventArgs e)
        {
            foreach (var sf in sfp.GetSessionFactories())
            {
                var localFactory = sf;
                LazySessionContext.Bind(
                    new Lazy<ISession>(() => BeginSession(localFactory)),
                    sf);
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
            foreach (var sf in sfp.GetSessionFactories())
            {
                var session = LazySessionContext.UnBind(sf);
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
            app.BeginRequest -= ContextBeginRequest;
            app.EndRequest -= ContextEndRequest;
        }
    }
}