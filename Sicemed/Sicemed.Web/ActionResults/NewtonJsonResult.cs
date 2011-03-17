using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sicemed.Web.ActionResults
{
    public class NewtonJsonResult : JsonResult
    {
        public NewtonJsonResult(JsonResult result)
        {
            this.ContentEncoding = result.ContentEncoding;
            this.Data = result.Data;
            this.ContentType = result.ContentType;
            this.JsonRequestBehavior = result.JsonRequestBehavior;
        }

        /// <summary>
        /// El código es el mismo de ASP.NET MVC 3, cambie el método que escribe en el response
        /// http://aspnet.codeplex.com/SourceControl/changeset/view/63930#266491
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.Converters.Add(new IsoDateTimeConverter());
                response.Write(JsonConvert.SerializeObject(this.Data, Formatting.None, settings));                
            }
        }
        
    }
}