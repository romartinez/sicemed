using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core.Logging;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication {
        private static IWindsorContainer _container;
        private static ILogger _logger;

        public static IWindsorContainer Container { get { return _container; } }
        public static ILogger Logger { get { return _logger; } }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //Windsor
            BootstrapContainer();
        }

        protected void Application_End() {
            Container.Dispose();
        }

        protected void Application_Error(object sender, EventArgs args) {
            if (Logger.IsFatalEnabled)
                _logger.FatalFormat("An unhandled error happened. Exc: {0}", Server.GetLastError());
        }

        private static void BootstrapContainer() {
            _container = new WindsorContainer()
                .Install(FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            _logger = _container.Resolve<ILogger>();
        }
    }
}