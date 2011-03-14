using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using Agatha.ServiceLayer;
using Castle.Core.Logging;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Newtonsoft.Json;
using Sicemed.Services;
using Sicemed.Services.Handlers;
using Sicemed.Services.Processors;
using Sicemed.Services.RR;
using Sicemed.Web.ModelBinders;
using Sicemed.Web.Models;
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

            System.Web.Mvc.ModelBinders.Binders.DefaultBinder = new ComplexObjectModelBinder();

            //Windsor
            BootstrapContainer();

            //Agatha
            BootstrapAgatha();
        }

        protected void Application_Error(object sender, EventArgs e) {
            if (_logger.IsFatalEnabled)
                _logger.FatalFormat("Error no atrapado. Exc: {0}", Server.GetLastError());
        }

        protected void Application_End() {
            _container.Dispose();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
            // Get the authentication cookie
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = Context.Request.Cookies[cookieName];

            // If the cookie can't be found, don't issue the ticket
            if (authCookie == null) return;

            // Get the authentication ticket and rebuild the principal 
            // & identity
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var roles = authTicket.UserData.Split(new[] { '|' });
            var identity = new IdentityBase(authTicket.Name);
            var extendedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(authTicket.UserData);
            identity.ExtendedData = extendedData;
            var userPrincipal = new PrincipalBase(identity, roles);
            Context.User = userPrincipal;
        }

        private static void BootstrapContainer() {
            _container = new WindsorContainer()
                .Install(FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            _logger = _container.Resolve<ILogger>();
        }

        private static void BootstrapAgatha() {
            //Para cambiar a otro proveedor de DMS, o a WCF modificar aquí
            var config = new ServiceLayerAndClientConfiguration(
                    typeof(NHibernateBaseRequestHandler<BaseRequest, BaseResponse>).Assembly,
                    typeof(BaseRequest).Assembly,
                    new CastleContainer(_container)) {
                        RequestProcessorImplementation = typeof(ClientExceptionRequestProcessor),
                        RequestDispatcherImplementation = typeof(UserAwareRequestDispatcher),
                        AsyncRequestDispatcherImplementation = typeof(AsyncUserAwareRequestDispatcher)
                    };
            config.Initialize();
        }
    }
}