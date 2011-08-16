using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class UrlHelperExtensions
    {
        public static T Deserialize<T>(string hexEncodedObject)
        {
            byte[] bytes = HexEncoding.GetBytes(hexEncodedObject);
            string jsonFormatted = Encoding.ASCII.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(jsonFormatted);
        }

        public static string Serialize(object @object)
        {
            string jsonFormatted = JsonConvert.SerializeObject(@object);
            byte[] bytes = Encoding.ASCII.GetBytes(jsonFormatted);
            return HexEncoding.ToString(bytes);
        }

        public static string Serialize(this UrlHelper urlHelper, object @object)
        {
            return Serialize(@object);
        }
    }
}