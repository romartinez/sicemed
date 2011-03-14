using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sicemed.Web.Controllers;

namespace Sicemed.Web.Plumbing.Installers {
    public class ControllersInstaller : IWindsorInstaller {
        public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.Register(FindControllers().Configure(ConfigureControllers()));
        }

        private ConfigureDelegate ConfigureControllers() {
            return c => c.Named(c.ServiceType.Name)
                            .LifeStyle.Transient;
        }

        private BasedOnDescriptor FindControllers() {
            return AllTypes.FromThisAssembly()
                .BasedOn<IController>()
                .If(Component.IsInSameNamespaceAs<HomeController>())
                .If(t => t.Name.EndsWith("Controller"));
        }
    }
}