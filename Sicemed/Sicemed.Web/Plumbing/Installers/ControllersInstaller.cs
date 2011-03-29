using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sicemed.Web.Areas.Public.Controllers;

namespace Sicemed.Web.Plumbing.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(FindControllers().Configure(ConfigureControllers()));
        }

        #endregion

        private ConfigureDelegate ConfigureControllers()
        {
            return c => c.Named(c.ServiceType.Name)
                            .LifeStyle.Transient;
        }

        private BasedOnDescriptor FindControllers()
        {
            return AllTypes.FromThisAssembly()
                .BasedOn<IController>()                
                .If(t => t.Name.EndsWith("Controller"));
        }
    }
}