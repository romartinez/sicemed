using Agatha.Common;
using Castle.Core.Logging;

namespace Sicemed.Web.Plumbing {
    public class BaseController : Controller{
        public IRequestDispatcher RequestDispatcher { get; set; }
        public IAsyncRequestDispatcher AsyncRequestDispatcher { get; set; }
        public ILogger Logger { get; set; }
    }
}