using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Castle.Core.Logging;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Sicemed.Web.Plumbing;
using Sicemed.Web.Plumbing.ModelBinders;
using Sicemed.Web.Plumbing.Queries.Usuarios;

namespace Sicemed.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer _container;
        public static IWindsorContainer Container
        {
            get { return _container; }
        }

        private static ILogger _logger = NullLogger.Instance;
        public static ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.DefaultBinder = new ComplexObjectModelBinder();

            //Windsor
            BootstrapContainer();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (_logger.IsFatalEnabled)
                _logger.FatalFormat("Error no atrapado. Exc: {0}", Server.GetLastError());
        }

        protected void Application_End()
        {
            _container.Dispose();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (authCookie == null) return;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var id = Convert.ToInt64(authTicket.UserData);
            var usuario = Container.Resolve<IUsuariosPorIdConRolesYPermisosQuery>().Execute(id);
            Context.User = usuario;
        }

        private static void BootstrapContainer()
        {
            _container = new WindsorContainer();
            _container.AddFacility<TypedFactoryFacility>();            
            _container.Install(FromAssembly.This());
            
            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            _logger = _container.Resolve<ILogger>();
        }
    }
}