﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core.Logging;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Combres;
using CommonServiceLocator.WindsorAdapter;
using DataAnnotationsExtensions.ClientValidation;
using Microsoft.Practices.ServiceLocation;
using SICEMED.Web.Infrastructure.Windsor.Facilities;
using Sicemed.Web.Infrastructure;
using Combres.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using ILogger = Castle.Core.Logging.ILogger;

namespace SICEMED.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
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
            routes.AddCombresRoute("Combres");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            _container = new WindsorContainer();
            _container.AddFacility<TypedFactoryFacility>();
            _container.Install(FromAssembly.This());

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));

            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();

            _logger = ServiceLocator.Current.GetInstance<ILogger>();
            ServiceLocator.Current.GetInstance<IApplicationInstaller>().Install(NHibernateFacility.BuildDatabaseConfiguration());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_container.Kernel));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if(_logger.IsFatalEnabled)
                _logger.FatalFormat("Error no atrapado. Exc: {0}", Server.GetLastError());
        }
    }
}