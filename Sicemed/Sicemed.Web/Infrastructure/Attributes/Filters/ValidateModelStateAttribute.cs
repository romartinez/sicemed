using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Exceptions;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
                throw new ValidationErrorException(filterContext.Controller.ViewData.ModelState);

            base.OnActionExecuting(filterContext);
        }
    }
}