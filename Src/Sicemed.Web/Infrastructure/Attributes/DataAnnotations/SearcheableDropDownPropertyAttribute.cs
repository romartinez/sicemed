using System;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SearcheableDropDownPropertyAttribute: Attribute 
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public SearcheableDropDownPropertyAttribute(string actionName, string controllerName) 
        {
            ActionName = actionName;
            ControllerName = controllerName;
        }
    }
}