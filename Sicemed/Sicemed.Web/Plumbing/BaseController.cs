using System.Web.Mvc;
using Castle.Core.Logging;
using Newtonsoft.Json;
using NHibernate;

namespace Sicemed.Web.Plumbing
{
    public class BaseController : Controller
    {
        public ILogger Logger { get; set; }
        public ISessionFactory SessionFactory { get; set; }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return Json(data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            var result = new JsonResult() { ContentEncoding = System.Text.Encoding.UTF8, ContentType = "application/json", JsonRequestBehavior = behavior };
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            result.Data = JsonConvert.SerializeObject(data, Formatting.None, settings);
            return result;
        }
    }
}