using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SICEMED.Web.Infrastructure.Windsor.Facilities;

namespace SICEMED.Web.Infrastructure.Windsor.Installers
{
    public class NHibernateInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<NHibernateFacility>();
        }

        #endregion
    }
}