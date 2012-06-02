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
            var display = attributes.SingleOrDefault(a => typeof(DisplayAttribute) == a.GetType());
            if (display != null)
            {
                var displayAttribute = ((DisplayAttribute) display);
                data.Watermark = displayAttribute.Prompt;
                data.Description = displayAttribute.Description;                
            }

            var dropDown = attributes.SingleOrDefault(a => typeof(DropDownPropertyAttribute) == a.GetType());
            if (dropDown != null) data.AdditionalValues.Add("DropDownProperty", ((DropDownPropertyAttribute) dropDown).PropertyName);

            return data;
        }
    }
}