﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Castle.Core.Logging;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CommonServiceLocator.WindsorAdapter;
using DataAnnotationsExtensions.ClientValidation;
using Microsoft.Practices.ServiceLocation;
using SICEMED.Web.Infrastructure.Windsor.Facilities;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Providers.FilterAtrribute;
using WebExtensions = Combres.WebExtensions;

namespace SICEMED.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private static ILogger _logger = NullLogger.Instance;
        private static WindsorContainer _container;

        public static WindsorContainer Container
        {
            get { return _container; }
        }

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
            AreaRegistration.RegisterAllAreas();

            WebExtensions.AddCombresRoute(routes, "Combres");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            DefaultModelBinder.ResourceClassKey = "Messages";
            ValidationExtensions.ResourceClassKey = "Messages";

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            _container = new WindsorContainer();
            _container.AddFacility<TypedFactoryFacility>();
            _container.Install(FromAssembly.This());

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));

            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();

            _logger = ServiceLocator.Current.GetInstance<ILogger>();
            ServiceLocator.Current.GetInstance<IApplicationInstaller>().Install(
                NHibernateFacility.BuildDatabaseConfiguration());
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_container.Kernel));
            FilterProviders.Providers.Add(new WindsorFilterAttributeFilterProvider(_container));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            _logger.Fatal("Error Fatal.", exception);

            Response.Clear();

            var httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        action = "HttpError404";
                        break;
                    case 500:
                        // server error
                        action = "HttpError500";
                        break;
                    default:
                        action = "General";
                        break;
                }

                // clear error on server
                Server.ClearError();

                Response.Redirect(String.Format("/Error/{0}/?message={1}", action, exception.Message));
            }
        }
    }
}