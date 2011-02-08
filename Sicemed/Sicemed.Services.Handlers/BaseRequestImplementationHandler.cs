using Agatha.Common;
using Agatha.ServiceLayer;

namespace Sicemed.Services.Handlers {
    public abstract class BaseRequestImplementationHandler<TRequest, TResponse> 
        : RequestHandler<TRequest, TResponse> 
        where TRequest : Request 
        where TResponse : Response {
    }
}
