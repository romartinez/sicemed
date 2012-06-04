using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EfficientlyLazy.Crypto;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Providers.Cache;
using Sicemed.Web.Infrastructure.Queries;
using Sicemed.Web.Infrastructure.Services;

namespace SICEMED.Web.Infrastructure.Windsor.Installers
{
    public class DependenciesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Application Services
            container.Register(
                //Singletons
                Component.For<IFormAuthenticationStoreService>().ImplementedBy<FormAuthenticationStoreService>().
                    LifeStyle.Singleton,
                Component.For<ICryptoEngine>().Instance(new RijndaelEngine("S3CR3t0.3spC14L")).LifeStyle.Singleton,
                Component.For<IMembershipMailer>().ImplementedBy<MembershipMailer>().LifeStyle.Transient,
                Component.For<ISecurityService>().ImplementedBy<SecurityService>().LifeStyle.Singleton,
                //Providers
                Component.For<ICacheProvider>().ImplementedBy<NullCacheProvider>().LifeStyle.Singleton,
                //Menu Provider
                //Queries
                AllTypes.FromThisAssembly().BasedOn<IQuery>()
                    .WithService.DefaultInterface().Configure(x=>x.LifeStyle.Transient),                
                Component.For<IApplicationInstaller>().ImplementedBy<ApplicationInstaller>().LifeStyle.Singleton,
                //Transients
                Component.For<IMembershipService>().ImplementedBy<MembershipService>().LifeStyle.Transient
                );
        }

        #endregion
    }
}