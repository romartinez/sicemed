using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sicemed.Web.Services.ApplicationServices.Account;

namespace Sicemed.Web.Plumbing.Installers
{
    public class ApplicationServicesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().Pick()
                                   .If(Component.IsInSameNamespaceAs<IFormsAuthenticationApplicationService>())
                                   .Configure(c => c.Named(c.ServiceType.Name)
                                                       .LifeStyle.Transient)
                                   .WithService.DefaultInterface());
        }

        #endregion
    }
}