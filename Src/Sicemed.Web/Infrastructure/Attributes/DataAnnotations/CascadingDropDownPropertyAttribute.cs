namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    public class CascadingDropDownPropertyAttribute: DropDownPropertyAttribute 
    {
        public string ParentPropertyName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ParameterName { get; set; }

        public CascadingDropDownPropertyAttribute(string propertyName, string parentPropertyName, string actionName, string controllerName, string parameterName = "id") 
            : base(propertyName)
        {
            ParentPropertyName = parentPropertyName;
            ActionName = actionName;
            ControllerName = controllerName;
            ParameterName = parameterName;
        }
    }
}