using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace SICEMED.Web.Infrastructure.Windsor.Installers
{
    public class LoggerInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<LoggingFacility>(
                f => f.LogUsing(LoggerImplementation.Log4net).WithConfig("log4net.config"));            
        }

        #endregion
    }
}