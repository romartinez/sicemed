using System.Text.RegularExpressions;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class StringExtensions
    {
        public static string SanitanizeForFileName(this string value)
        {
            return Regex.Replace(Regex.Replace(value, @"\s", "_"), @"[^\w]", string.Empty);
        }
    }
}