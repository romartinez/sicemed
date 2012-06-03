using System.Collections.Generic;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class AjaxMessagesFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (filterContext.Controller.TempData.ContainsKey(BaseController.MESSAGES_KEY))
                {
                    var storedMessages = (List<ResponseMessage>)filterContext.Controller.TempData[BaseController.MESSAGES_KEY];
                    filterContext.HttpContext.Response.AddHeader("X-ResponseMessages", Json.SerializeObject(storedMessages));
                }
            }
        }
    }
}