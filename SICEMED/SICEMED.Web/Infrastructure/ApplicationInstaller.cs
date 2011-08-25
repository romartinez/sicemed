using System;
using System.Web;
using Castle.Core.Logging;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Exceptions;
using NHibernate.Tool.hbm2ddl;
using Sicemed.Web.Infrastructure.Providers.Session;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure
{
    public interface IApplicationInstaller
    {
        void Install(Configuration config);
    }

    public class ApplicationInstaller : IApplicationInstaller
    {
        public virtual ISessionFactory SessionFactory { get; set; }
        public virtual ILogger Logger { get; set; }
        public virtual IMembershipService MembershipService { get; set; }

        public ApplicationInstaller()
        {
            Logger = NullLogger.Instance;
        }

        public void Install(Configuration config)
        {
            var session = SessionFactory.GetCurrentSession() ?? SessionFactory.OpenSession();
            using(var importSession = SessionFactory.OpenSession(session.Connection))
            {
                if(HttpContext.Current != null)
                    LazySessionContext.Bind(new Lazy<ISession>(() => importSession), SessionFactory);
                else
                    CurrentSessionContext.Bind(importSession);

                Logger.InfoFormat("Checking if the application is installed.");
                try
                {
                    var param = importSession.Get<Parametro>(Parametro.Keys.APP_IS_INITIALIZED);
                    var isInitialized = param == null || param.Get<bool>();
                    Logger.DebugFormat("The parameter for the DB Installed is: {0}", isInitialized);
                    if(!isInitialized)
                    {
                        Initialize(config, importSession);
                    }
                }
                catch(GenericADOException)
                {
                    Logger.WarnFormat("The DB isn't initialized, generating it.");
                    //Check if the DB is created
                    Initialize(config, importSession);
                }
                if(HttpContext.Current != null)
                    LazySessionContext.UnBind(SessionFactory);
                else
                    CurrentSessionContext.Unbind(SessionFactory);
            }
        }

        private void Initialize(Configuration config, ISession session)
        {
            Logger.InfoFormat("Installing the application.");
            new SchemaExport(config).Execute(false, true, false, session.Connection, null);
            //No permite Tx anidadas
            CrearAdministrador();
            //Lo guardo al parametro nuevo.
            //using (var tx = session.BeginTransaction())
            //{
            //    var param = new Parametro() { Key = Parametro.Keys.APP_IS_INITIALIZED };
            //    param.Set(true);

            //    session.Save(param);
            //    tx.Commit();
            //}
        }

        private void CrearAdministrador()
        {
            var usuario = new Usuario() { Nombre = "admin" };
            usuario.AgregarRol(Rol.Administrador);
            MembershipService.CreateUser(usuario, "admin@admin.com", "test");
        }
    }
}