using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace Sicemed.Web.Helpers {
// ReSharper restore CheckNamespace
    public static class UrlHelperExtensions {
        public static T Deserialize<T>(string hexEncodedObject) {
            var bytes = HexEncoding.GetBytes(hexEncodedObject);
            var jsonFormatted = Encoding.ASCII.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(jsonFormatted);            
        }

        public static string Serialize(object @object) {
            var jsonFormatted = JsonConvert.SerializeObject(@object);
            var bytes = Encoding.ASCII.GetBytes(jsonFormatted);
            return HexEncoding.ToString(bytes);            
        }

        public static string Serialize(this UrlHelper urlHelper, object @object) {
            return Serialize(@object);
        }
    }
}