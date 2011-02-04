using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sicemed.Web.Plumbing.Facilities;

namespace Sicemed.Web.Plumbing.Installers {
    public class PersistenceInstaller : IWindsorInstaller {
        public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.AddFacility<PersistenceFacility>();
        }
    }
}