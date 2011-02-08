using Agatha.Common;
using Agatha.Common.Caching;
using Sicemed.Services.RR;
using Sicemed.Web.ApplicationServices.Account;

namespace Sicemed.Web.Plumbing {
    public class AsyncUserAwareRequestDispatcher : AsyncRequestDispatcher {
        private readonly IUserDiscoveringApplicationService _userDiscoveringApplicationService;


        public AsyncUserAwareRequestDispatcher(IAsyncRequestProcessor requestProcessor, ICacheManager cacheManager, IUserDiscoveringApplicationService userDiscoveringApplicationService) 
            : base(requestProcessor, cacheManager) {
            _userDiscoveringApplicationService = userDiscoveringApplicationService;
        }

        protected override void BeforeSendingRequests(System.Collections.Generic.IEnumerable<Request> requestsToProcess) {
            foreach(var request in requestsToProcess) {
                if (typeof(AuthenticatedRequest).IsAssignableFrom(request.GetType())) {
                    var authenticatedRequest = (AuthenticatedRequest) request;
                    authenticatedRequest.Principal = _userDiscoveringApplicationService.GetCurrentUser();
                }
            }
            base.BeforeSendingRequests(requestsToProcess);
        }
    }
}