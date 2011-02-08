using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sicemed.Web.ApplicationServices.Account;

namespace Sicemed.Web.Plumbing.Installers {
    public class ApplicationServicesInstaller : IWindsorInstaller {
        public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.Register(AllTypes.FromThisAssembly().Pick()
                                .If(Component.IsInSameNamespaceAs<IFormsAuthenticationApplicationService>())
                                .Configure(c => c.Named(c.ServiceType.Name)
                                                .LifeStyle.Transient)
                                                .WithService.DefaultInterface());
        }
    }
}