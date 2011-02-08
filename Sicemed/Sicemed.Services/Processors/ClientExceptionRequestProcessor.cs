using System;
using System.Reflection;
using Agatha.Common;
using Agatha.Common.Caching;
using Agatha.ServiceLayer;
using Sicemed.Services.Exceptions;

namespace Sicemed.Services.Processors {
    public class ClientExceptionRequestProcessor : PerformanceLoggingRequestProcessor {
        private readonly ServiceLayerConfiguration _serviceLayerConfiguration;

        public ClientExceptionRequestProcessor(ServiceLayerConfiguration serviceLayerConfiguration, ICacheManager cacheManager) 
            : base(serviceLayerConfiguration, cacheManager) {
            _serviceLayerConfiguration = serviceLayerConfiguration;
        }

        protected override Response CreateExceptionResponse(IRequestHandler handler, System.Exception exception) {
            var response = handler.CreateDefaultResponse();
            response.Exception = new ActivableExceptionInfo(exception);
            this.SetExceptionType(response, exception);
            return response;
        }

        private void SetExceptionType(Response response, Exception exception) {
            var type = exception.GetType();
            if (type.IsAssignableFrom(_serviceLayerConfiguration.BusinessExceptionType)) {
                response.ExceptionType = ExceptionType.Business;
                this.SetExceptionFaultCode(exception, response.Exception);
            } else if (type.IsAssignableFrom(_serviceLayerConfiguration.SecurityExceptionType)) {
                response.ExceptionType = ExceptionType.Security;
            } else {
                response.ExceptionType = ExceptionType.Unknown;
            }
        }

        private void SetExceptionFaultCode(Exception exception, ExceptionInfo exceptionInfo) {
            PropertyInfo property = exception.GetType().GetProperty("FaultCode");
            if (((property != null) && property.CanRead) && property.PropertyType.Equals(typeof(string))) {
                exceptionInfo.FaultCode = (string)property.GetValue(exception, null);
            }
        }
    }
}
