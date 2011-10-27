using System;
using System.Web;
using NHibernate;
using NHibernate.Context;

namespace Sicemed.Web.Infrastructure.HttpModules
{
    public class NHibernateSessionModule : IHttpModule
    {
        private HttpApplication app;
        public const string NH_SESSION_FACTORY_KEY = "NH_SESSION_FACTORY_KEY";

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            app = context;
            context.BeginRequest += ContextBeginRequest;
            context.EndRequest += ContextEndRequest;
        }

        public void Dispose()
        {
            app.BeginRequest -= ContextBeginRequest;
            app.EndRequest -= ContextEndRequest;
        }

        #endregion

        private void ContextBeginRequest(object sender, EventArgs e)
        {
            var sf = (ISessionFactory) app.Context.Application[NH_SESSION_FACTORY_KEY];
            CurrentSessionContext.Bind(BeginSession(sf));
        }

        private static ISession BeginSession(ISessionFactory sf)
        {
            var session = sf.OpenSession();
            session.BeginTransaction();
            return session;
        }

        private void ContextEndRequest(object sender, EventArgs e)
        {
            var sf = (ISessionFactory) app.Context.Application[NH_SESSION_FACTORY_KEY];
            var session = CurrentSessionContext.Unbind(sf);            
            EndSession(session);
        }

        private void EndSession(ISession session)
        {
            var exc = app.Server.GetLastError();
            if (exc == null && session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Commit();
            }
            session.Dispose();
        }
    }
}