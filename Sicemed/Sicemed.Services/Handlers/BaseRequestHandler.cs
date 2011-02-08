using System;
using System.ComponentModel.DataAnnotations;
using System.Security;
using Agatha.Common;
using Agatha.ServiceLayer;
using Castle.Core.Logging;
using Sicemed.Common.Helpers;
using Sicemed.Services.RR;
using System.Linq;


namespace Sicemed.Services.Handlers {
    public abstract class BaseRequestHandler<TRequest, TResponse> : RequestHandler<TRequest, TResponse>
        where TRequest : BaseRequest
        where TResponse : BaseResponse {

        public virtual ILogger Logger { get; set; }

        public override Response Handle(Request request) {
            if (Logger.IsDebugEnabled) Logger.DebugFormat("Handling Request: {0}", request);

            //Validate the request (required, etc..)
            if (Logger.IsDebugEnabled) Logger.DebugFormat("Validating the Request.");
            var validationErrors = ValidationHelper.Validate(request);
            if (validationErrors.Any()) {
                var validationErrorsString = string.Join(" || ", validationErrors.Select(e => string.Format("{0}: {1}", string.Join(", ", e.MemberNames),e.ErrorMessage)));
                if (Logger.IsErrorEnabled)
                    Logger.ErrorFormat("There was errrors validating the Request: {0}. Errors: {1}", request, validationErrorsString);
                throw new ValidationException("The request is invalid. Errors: " + validationErrorsString);
            }

            if (request is AuthenticatedRequest && !IsAuthenticated(request as AuthenticatedRequest))
                throw new SecurityException("The current request must be authenticated, and those credential wasn't suppplied.");

            Response response = null;
            try {
                response = base.Handle(request);
            } catch (Exception ex) {
                if (Logger.IsFatalEnabled) Logger.FatalFormat("Error handling request. Exc:{0}", ex);
                throw;
            }

            if (Logger.IsDebugEnabled) Logger.DebugFormat("Sending Response: {0}", response);

            return response;
        }

        private bool IsAuthenticated(AuthenticatedRequest request) {
            return  request.Principal != null 
                &&  request.Principal.Identity != null 
                &&  request.Principal.Identity.IsAuthenticated;
        }
    }
}
