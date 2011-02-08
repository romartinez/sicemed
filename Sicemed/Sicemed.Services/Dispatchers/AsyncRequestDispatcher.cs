using Agatha.Common;
using Agatha.Common.Caching;

namespace Sicemed.Services.Dispatchers {
    public class AsyncRequestDispatcher : Agatha.Common.AsyncRequestDispatcher{
        public AsyncRequestDispatcher(IAsyncRequestProcessor requestProcessor, ICacheManager cacheManager) 
            : base(requestProcessor, cacheManager) {}
    }
}
