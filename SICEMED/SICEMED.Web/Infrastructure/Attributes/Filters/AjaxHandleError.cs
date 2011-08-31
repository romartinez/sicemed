using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using Microsoft.Practices.ServiceLocation;
using Sicemed.Web.Infrastructure.Exceptions;
using log4net;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    /// <summary>
    /// Algo parecido a:
    /// http://stackoverflow.com/questions/3274808/how-can-i-make-handleerrorattribute-work-with-ajax
    /// pero que usa JSON
    /// </summary>
    public class AjaxHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var log = ServiceLocator.Current.GetInstance<ILogger>();
            if (log.IsFatalEnabled)
            {
                //NOTE: No adan si usamos FatalFormat
                var logStr = "Se ha producido un error en la llamada.\n Method: {0} \nArgs: {1}\n Exc: {2}";
                var jsonArgs = filterContext.RouteData.ToString();
                logStr = string.Format(logStr, filterContext.Controller, jsonArgs, filterContext.Exception);
                log.Fatal(logStr);
            }

            // Execute the normal exception handling routine
            base.OnException(filterContext);

            // Verify if AJAX request
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {

                var result = new JsonResult();
                if (filterContext.Exception is ToClientException || HttpContext.Current.IsDebuggingEnabled)
                {
                    result.Data = ResponseMessage.Error(filterContext.Exception.Message, filterContext.Exception);
                }
                else
                {
                    result.Data = ResponseMessage.Error();
                }

                filterContext.HttpContext.Response.Clear();
                filterContext.Result = result;
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }
    }
}