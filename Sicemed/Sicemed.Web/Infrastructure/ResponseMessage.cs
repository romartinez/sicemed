using System;
using System.Collections;

namespace Sicemed.Web.Infrastructure
{
    public class ResponseMessage
    {
        public bool IsSuccessful { get; set; }
        public string Description { get; set; }
        public IDictionary Data { get; set; }

        public static ResponseMessage Success(string description = null)
        {
            return new ResponseMessage {IsSuccessful = true, Description = description};
        }

        public static ResponseMessage Error(string description = null, Exception exception = null)
        {
            return new ResponseMessage
                   {IsSuccessful = false, Description = description, Data = exception != null ? exception.Data : null};
        }
    }
}