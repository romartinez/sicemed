using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Castle.Core.Logging;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Newtonsoft.Json;
using Sicemed.Web.Plumbing;
using Sicemed.Web.Plumbing.ModelBinders;

namespace Sicemed.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer _container;
        private static ILogger _logger;

        public static IWindsorContainer Container
        {
            get { return _container; }
        }

        public static ILogger Logger
        {
            get { return _logger; }
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
            // Get the authentication cookie
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            // If the cookie can't be found, don't issue the ticket
            if (authCookie == null) return;

            // Get the authentication ticket and rebuild the principal 
            // & identity
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            string[] roles = authTicket.UserData.Split(new[] {'|'});
            var identity = new IdentityBase(authTicket.Name);
            var extendedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(authTicket.UserData);
            identity.ExtendedData = extendedData;
            var userPrincipal = new PrincipalBase(identity, roles);
            Context.User = userPrincipal;
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