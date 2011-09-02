using System;
using System.ComponentModel;
using System.Reflection;
using Resources;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class EnumReaderExtensions
    {
        public static string Get(this Enum status)
        {
            var translated = Resource.ResourceManager.GetString(status.GetType().Name + "_" + status);
            if(string.IsNullOrWhiteSpace(translated)) return GetDescription(status);
            return translated;
        }


        /// <summary>
        /// From: http://geekswithblogs.net/rakker/archive/2006/05/19/78952.aspx/ 
        /// Returns a string containing the description attribute value or the enum's name if no description is provided or tostring
        /// if the object is not an enum. Note that if you call this function without passing an enum, you'll have a performance penalty
        /// because of exception handling. The to string method will be called and you'll get a string result, but it'll be slow.
        /// </summary>
        /// <param name="value">The enum value for which to get the description.</param>
        /// <returns>a string containing the description attribute value or the enum's name if no description is provided or tostring
        /// if the object is not an enum.</returns>
        public static string GetDescription(object value)
        {
            string result = string.Empty;
            if(value != null)
            {
                result = value.ToString();
                // Get the type from the object.
                Type type = value.GetType();
                try
                {
                    result = Enum.GetName(type, value);
                    // Get the member on the type that corresponds to the value passed in.
                    FieldInfo fieldInfo = type.GetField(result);
                    // Now get the attribute on the field.
                    object[] attributeArray = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    DescriptionAttribute attribute = null;
                    if(attributeArray.Length > 0)
                    {
                        attribute = (DescriptionAttribute)attributeArray[0];
                    }
                    if(attribute != null)
                    {
                        result = attribute.Description;
                    }
                }
                catch(ArgumentNullException)
                {
                    //We shouldn't ever get here, but it means that value was null, so we'll just go with the default.
                    result = string.Empty;
                }
                catch(ArgumentException)
                {
                    //we didn't have an enum.
                    result = value.ToString();
                }
            }
            // Return the description.
            return result;
        }		

    }
}