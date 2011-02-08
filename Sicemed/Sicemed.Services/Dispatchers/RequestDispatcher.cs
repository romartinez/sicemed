using Agatha.Common;
using Agatha.Common.Caching;

namespace Sicemed.Services.Dispatchers {
    public class RequestDispatcher : Agatha.Common.RequestDispatcher{
        public RequestDispatcher(IRequestProcessor requestProcessor, ICacheManager cacheManager) 
            : base(requestProcessor, cacheManager) { }
    }
}
