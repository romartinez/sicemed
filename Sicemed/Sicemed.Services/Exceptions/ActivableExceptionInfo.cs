using System;
using Agatha.Common;

namespace Sicemed.Services.Exceptions {
    public class ActivableExceptionInfo : ExceptionInfo {
        public ActivableExceptionInfo(Exception exception) : base(exception) {
            var exceptionType = exception.GetType();
            this.Type = string.Format("{0}, {1}", exceptionType.FullName, exceptionType.Assembly.FullName);
            if (exception.InnerException != null) {
                this.InnerException = new ActivableExceptionInfo(exception.InnerException);
            }
        }
    }
}
