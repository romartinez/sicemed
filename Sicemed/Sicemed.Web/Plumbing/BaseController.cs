using System.Web.Mvc;
using Castle.Core.Logging;
using NHibernate;

namespace Sicemed.Web.Plumbing
{
    public class BaseController : Controller
    {
        public ILogger Logger { get; set; }
        public ISessionFactory SessionFactory { get; set; }
    }
}