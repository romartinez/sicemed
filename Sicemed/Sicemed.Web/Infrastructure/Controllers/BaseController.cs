using System.Text;
using System.Web.Mvc;
using Castle.Core.Logging;
using Sicemed.Web.Infrastructure.ActionResults;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Controllers
{
    public class BaseController : Controller
    {
        private ILogger _logger;

        public ILogger Logger
        {
            get { return _logger ?? (_logger = NullLogger.Instance); }
            set { _logger = value; }
        }

        public IMembershipService MembershipService {get;set;}

        protected new Usuario User
        {
            get
            {
                return MembershipService.GetCurrentUser();   
            }
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding,
                                           JsonRequestBehavior behavior)
        {
            return new NewtonJsonResult(base.Json(data, contentType, contentEncoding, behavior));
        }
    }
}