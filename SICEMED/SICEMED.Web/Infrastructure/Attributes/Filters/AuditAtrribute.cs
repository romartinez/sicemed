using System.Web.Mvc;
using Castle.Core.Logging;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class AuditAtrribute : ActionFilterAttribute
    {
        public virtual ILogger Logger { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Logger.IsDebugEnabled)
                Logger.DebugFormat("Executing action {0} of the controller {1}",
                                   filterContext.ActionDescriptor.ActionName,
                                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);

            base.OnActionExecuted(filterContext);
        }
    }
}