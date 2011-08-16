using System.Web.Mvc;
using Castle.Core.Logging;
using Sicemed.Web.Infrastructure.ActionResults;

namespace SICEMED.Web.Infrastructure
{
    public class BaseController : Controller
    {
        private ILogger _logger = NullLogger.Instance;

        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }                

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new NewtonJsonResult(base.Json(data, contentType, contentEncoding, behavior));
        }
    }
}