using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sicemed.Web.Plumbing.Queries;
using Sicemed.Web.Services.ApplicationServices.Cuenta;

namespace Sicemed.Web.Plumbing.Installers
{
    public class DependenciesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .Register(AllTypes.FromThisAssembly().Pick()
                                    .If(Component.IsInSameNamespaceAs<IFormsAuthenticationApplicationService>())
                                    .Configure(c => c.Named(c.ServiceType.Name)
                                                        .LifeStyle.Transient)
                                    .WithService.DefaultInterface());
            container.Register(AllTypes.FromThisAssembly().BasedOn<IQuery>()                                
                                .Configure(c => c.Named(c.ServiceType.Name)
                                                    .LifeStyle.Transient)
                                .WithService.DefaultInterface());
        }

        #endregion
    }
}