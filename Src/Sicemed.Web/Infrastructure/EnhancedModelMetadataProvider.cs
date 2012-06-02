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
            if (dropDown != null) data.AdditionalValues.Add("DropDownProperty.PropertyName", ((DropDownPropertyAttribute)dropDown).PropertyName);

            var cascadingDropDown = attributes.SingleOrDefault(a => a is CascadingDropDownPropertyAttribute);
            if (cascadingDropDown != null)
            {
                data.AdditionalValues.Add("CascadingDropDownProperty.ParentPropertyName", ((CascadingDropDownPropertyAttribute)cascadingDropDown).ParentPropertyName);   
                data.AdditionalValues.Add("CascadingDropDownProperty.ActionName", ((CascadingDropDownPropertyAttribute)cascadingDropDown).ActionName);
                data.AdditionalValues.Add("CascadingDropDownProperty.ControllerName", ((CascadingDropDownPropertyAttribute)cascadingDropDown).ControllerName);
                data.AdditionalValues.Add("CascadingDropDownProperty.ParameterName", ((CascadingDropDownPropertyAttribute)cascadingDropDown).ParameterName);   
            }                

            return data;
        }
    }
}