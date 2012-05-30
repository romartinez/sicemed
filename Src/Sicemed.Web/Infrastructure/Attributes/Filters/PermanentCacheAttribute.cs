using System;
using System.Web;
using System.Web.Mvc;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class PermanentCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                var cache = filterContext.HttpContext.Response.Cache;
                cache.SetMaxAge(TimeSpan.FromDays(30));
                cache.SetLastModified(DateTime.Now);
                cache.SetExpires(DateTime.Now.AddDays(30)); // For HTTP 1.0 browsers
                cache.SetValidUntilExpires(true);
                cache.SetCacheability(HttpCacheability.Public);
                cache.SetRevalidation(HttpCacheRevalidation.None);
                cache.VaryByHeaders["Accept-Encoding"] = true;
                    // Tell proxy to cache different versions depending on Accept-Encoding
            }
            base.OnResultExecuted(filterContext);
        }
    }
}