using System;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DropDownPropertyAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public DropDownPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}