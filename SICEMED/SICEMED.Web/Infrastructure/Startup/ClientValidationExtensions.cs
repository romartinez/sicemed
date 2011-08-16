using Bootstrap.Extensions.StartupTasks;
using DataAnnotationsExtensions.ClientValidation;

namespace Sicemed.Web.Infrastructure.Startup
{
    public class ClientValidationExtensions : IStartupTask
    {
        public void Run()
        {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();
        }

        public void Reset() { }
    }
}