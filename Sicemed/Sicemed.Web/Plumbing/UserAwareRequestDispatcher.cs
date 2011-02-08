using System;
using System.Collections.Generic;
using System.Linq;
using Agatha.Common;
using Agatha.Common.Caching;
using Sicemed.Services.RR;
using Sicemed.Web.ApplicationServices.Account;

namespace Sicemed.Web.Plumbing {
    public class UserAwareRequestDispatcher : RequestDispatcher {
        private readonly IUserDiscoveringApplicationService _userDiscoveringApplicationService;

        public UserAwareRequestDispatcher(IRequestProcessor requestProcessor, ICacheManager cacheManager, IUserDiscoveringApplicationService userDiscoveringApplicationService) 
            : base(requestProcessor, cacheManager) {
            _userDiscoveringApplicationService = userDiscoveringApplicationService;
        }

        protected override void BeforeSendingRequests(IEnumerable<Request> requestsToProcess) {
            foreach(var request in requestsToProcess) {
                if (typeof(AuthenticatedRequest).IsAssignableFrom(request.GetType())) {
                    var authenticatedRequest = (AuthenticatedRequest) request;
                    authenticatedRequest.Principal = _userDiscoveringApplicationService.GetCurrentUser();
                }
            }
            base.BeforeSendingRequests(requestsToProcess);
        }

        protected override void BeforeReturningResponses(IEnumerable<Response> receivedResponses) {
            //If the responses are only one, we rethrow the exception so we can catch it on the client code.
            var responses = receivedResponses.ToList();
            if(responses.Count == 1 
                && responses[0] != null 
                && responses[0].Exception != null) {
                throw CreateException(responses[0].Exception);
            }
            base.BeforeReturningResponses(receivedResponses);
        }

        private Exception CreateException(ExceptionInfo exceptionInfo) {
            try {
                var type = Type.GetType(exceptionInfo.Type);
                if (exceptionInfo.InnerException != null) {
                    var innerException = CreateException(exceptionInfo.InnerException);
                    return (Exception) Activator.CreateInstance(type, exceptionInfo.Message, innerException);
                }
                return (Exception)Activator.CreateInstance(type, exceptionInfo.Message);
            }catch(Exception ex) {
                return new Exception(exceptionInfo.Message, ex);
            }
        }
    }
}