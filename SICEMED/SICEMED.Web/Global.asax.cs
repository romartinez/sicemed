﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bootstrap;
using Bootstrap.Windsor;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.StartupTasks;
using Castle.Core.Logging;
using Microsoft.Practices.ServiceLocation;
using Sicemed.Web.Infrastructure;

namespace SICEMED.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

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

            Bootstrapper.With.Windsor().And.AutoMapper().And.StartupTasks().Start();

            _logger = ServiceLocator.Current.GetInstance<ILogger>();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (_logger.IsFatalEnabled)
                _logger.FatalFormat("Error no atrapado. Exc: {0}", Server.GetLastError());
        }
    }
}