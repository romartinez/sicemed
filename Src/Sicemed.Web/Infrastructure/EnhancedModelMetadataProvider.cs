using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Infrastructure
{
    public class EnhancedModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(
                                     IEnumerable<Attribute> attributes,
                                     Type containerType,
                                     Func<object> modelAccessor,
                                     Type modelType,
                                     string propertyName)
        {

            var data = base.CreateMetadata(
                                 attributes,
                                 containerType,
                                 modelAccessor,
                                 modelType,
                                 propertyName);

            //Enhance the metadata with custom attributes
            var display = attributes.SingleOrDefault(a => a is DisplayAttribute);
            if (display != null)
            {
                var displayAttribute = ((DisplayAttribute)display);
                data.Watermark = displayAttribute.Prompt;
                data.Description = displayAttribute.Description;
            }

            var dropDown = attributes.SingleOrDefault(a => a is DropDownPropertyAttribute);
            if (dropDown != null)
            {
                var drop = ((DropDownPropertyAttribute) dropDown);
                data.AdditionalValues.Add("DropDownProperty.PropertyName", drop.PropertyName);
                var property = containerType.GetProperty(drop.PropertyName);
                if(property!= null)
                {
                    var attr = property.GetCustomAttributes(typeof(RequeridoAttribute), true);
                    data.IsRequired = attr.Any();
                }
            }

            var cascadingDropDown = attributes.SingleOrDefault(a => a is CascadingDropDownPropertyAttribute);
            if (cascadingDropDown != null)
            {
                var cascading = (CascadingDropDownPropertyAttribute) cascadingDropDown;
                data.AdditionalValues.Add("CascadingDropDownProperty.ParentPropertyName", cascading.ParentPropertyName);
                data.AdditionalValues.Add("CascadingDropDownProperty.ParentPrompt", cascading.ParentPrompt);
                data.AdditionalValues.Add("CascadingDropDownProperty.ActionName", cascading.ActionName);
                data.AdditionalValues.Add("CascadingDropDownProperty.ControllerName", cascading.ControllerName);
                data.AdditionalValues.Add("CascadingDropDownProperty.AreaName", cascading.AreaName);
                data.AdditionalValues.Add("CascadingDropDownProperty.ParameterName", cascading.ParameterName);
            }                

            var searchDropDown = attributes.SingleOrDefault(a => a is SearcheableDropDownPropertyAttribute);
            if (searchDropDown != null)
            {
                var search = (SearcheableDropDownPropertyAttribute)searchDropDown;
                data.AdditionalValues.Add("SearcheableDropDownPropertyAttribute.ActionName", search.ActionName);
                data.AdditionalValues.Add("SearcheableDropDownPropertyAttribute.ControllerName", search.ControllerName);
                data.AdditionalValues.Add("SearcheableDropDownPropertyAttribute.Template", search.Template);
                data.AdditionalValues.Add("SearcheableDropDownPropertyAttribute.DisplayProperty", search.DisplayProperty);
            }                

            return data;
        }
    }
}