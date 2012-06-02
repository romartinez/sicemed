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
            return new ResponseMessage
                       {
                           IsSuccessful = true, 
                           Description = description 
                                ?? "La operación se ha realizado con éxito."
                       };
        }

        public static ResponseMessage Error(string description = null, Exception exception = null)
        {
            return new ResponseMessage
                       {
                           IsSuccessful = false, 
                           Description = description 
                                ?? "Se ha producido un error inesperado, por favor inténtelo en unos instantes nuevamente.", 
                           Data = exception != null ? exception.Data : null
                       };
        }
    }
}