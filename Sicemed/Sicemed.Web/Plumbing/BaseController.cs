using System.Web.Mvc;
using Castle.Core.Logging;
using Newtonsoft.Json;
using NHibernate;
using Sicemed.Web.ActionResults;

namespace Sicemed.Web.Plumbing
{
    public class BaseController : Controller
    {
        public ILogger Logger { get; set; }
        public ISessionFactory SessionFactory { get; set; }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new NewtonJsonResult(base.Json(data, contentType, contentEncoding, behavior));
        }
    }
}