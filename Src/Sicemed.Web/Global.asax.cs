using System;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using AutoMapper;
using Castle.Core.Logging;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Combres;
using CommonServiceLocator.WindsorAdapter;
using DataAnnotationsExtensions.ClientValidation;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Transform;
using SICEMED.Web.Infrastructure.Windsor.Facilities;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.ModelBinders;
using Sicemed.Web.Infrastructure.Providers.FilterAtrribute;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels;
using ILogger = Castle.Core.Logging.ILogger;

namespace SICEMED.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private static ClinicaViewModel _clinica;
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

        public static ClinicaViewModel Clinica
        {
            get { return _clinica; }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private static void AddPaginaToRoute(RouteCollection routes, Pagina pagina)
        {
            routes.MapRoute(
                "Pagina - " + pagina.Nombre,
                pagina.Url == "/" ? string.Empty : pagina.Url, // URL with parameters
                new { controller = "Content", action = "Index", id = pagina.Id }
                );

            foreach (var hijo in pagina.Hijos)
            {
                AddPaginaToRoute(routes, hijo);
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            AreaRegistration.RegisterAllAreas();

            WebExtensions.AddCombresRoute(routes, "Combres");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Add the SEO Urls
            var sessionFactory = _container.Resolve<ISessionFactory>();
            using (var session = sessionFactory.OpenSession())
            {
                var paginas = session.QueryOver<Pagina>()
                                    .Fetch(x => x.Hijos).Eager
                                    .OrderBy(x => x.Orden).Asc
                                    .Where(x => x.Padre == null)
                                    .TransformUsing(Transformers.DistinctRootEntity).List();

                foreach (var pagina in paginas)
                {
                    AddPaginaToRoute(routes, pagina);
                }
            }

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Content", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            DefaultModelBinder.ResourceClassKey = "Messages";
            ValidationExtensions.ResourceClassKey = "Messages";

            var binderProvider = new InheritanceAwareModelBinderProvider
                            {
                                { typeof(ICustomBindeableProperties), new CustomBindeablePropertiesModelBinder() },
                                { typeof(DateTime), new DateTimeCurrentCultureModelBinder() },
                                { typeof(DateTime?), new DateTimeCurrentCultureModelBinder() },
                                { typeof(SearchFiltersViewModel), new SearchFiltersModelBinder()}
                            };

            ModelBinderProviders.BinderProviders.Add(binderProvider);



            RegisterGlobalFilters(GlobalFilters.Filters);

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

            //Automapper
            AutoMapperConfiguration.Configure();

            //MetadataProvider
            ModelMetadataProviders.Current = new EnhancedModelMetadataProvider();

            //Cargar la clinica
            CargarClinica();

            RegisterRoutes(RouteTable.Routes);
        }

        public static void ResetRoutes()
        {
            RouteTable.Routes.Clear();
            RegisterRoutes(RouteTable.Routes);
        }

        public static void CargarClinica(Clinica clinica = null)
        {
            if (clinica != null)
            {
                _clinica = ServiceLocator.Current.GetInstance<IMappingEngine>().Map<ClinicaViewModel>(clinica);
                return;
            }
            
            using (var session = ServiceLocator.Current.GetInstance<ISessionFactory>().OpenSession())
            {
                clinica = session.QueryOver<Clinica>().Take(1).List().FirstOrDefault();
                if (clinica == default(Clinica))
                    throw new ConfigurationErrorsException("No se encuentra ninguna clínica cargada en la base de datos.");

                _clinica = ServiceLocator.Current.GetInstance<IMappingEngine>().Map<ClinicaViewModel>(clinica);
            }
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
                Response.Redirect(String.Format("/Error/{0}/", action));
            }

            if(exception as SecurityException != null)
            {
                // clear error on server
                Server.ClearError();
                Response.Redirect(String.Format("/Error/Security/"));
            }
        }

        protected void Application_End()
        {
            if (_container != null) _container.Dispose();
        }

        public static bool IsUsingProxy
        {
            get
            {
                return Environment.MachineName == "WSFTYSDES002S";
            }
        }
    }
}