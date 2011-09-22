using System;
using System.IO;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Sicemed.Web.Infrastructure.NHibernate;
using log4net;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class Json
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Json));

        private static readonly JsonSerializer Serializer = new JsonSerializer
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new NHibernateContractResolver()
        };

        public static string SerializeObject(object objectToSerialize)
        {
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        Serializer.Serialize(jsonWriter, objectToSerialize);
                        return stringWriter.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled)
                    Log.ErrorFormat("Hubo un error al serializar el objeto tipo: '{0}' igual a '{1}'. Exc: {2}",
                        objectToSerialize != null ? objectToSerialize.GetType().ToString() : "(null)",
                        objectToSerialize != null ? objectToSerialize.ToString() : "(null)",
                        ex);
                return string.Empty;
            }
        }
    }
}