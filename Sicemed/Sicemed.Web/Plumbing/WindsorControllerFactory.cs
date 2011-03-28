using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace Sicemed.Web.Plumbing
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            //_kernel.ReleaseComponent(controller);
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            string controllerComponentName = controllerName + "Controller";
            
            return _kernel.Resolve<IController>(controllerComponentName);
        }
    }
}