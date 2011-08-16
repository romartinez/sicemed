using System.Web.Mvc;
using Bootstrap.Windsor;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;

namespace Sicemed.Web.Infrastructure.Startup
{
    public class WindsorStartupTask : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.AddFacility<TypedFactoryFacility>();
            container.Install(FromAssembly.This());
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}