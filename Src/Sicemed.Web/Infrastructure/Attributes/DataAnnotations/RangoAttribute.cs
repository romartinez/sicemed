using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    public class RangoAttribute : RangeAttribute
    {         
        const string RESOURCE_NAME = "RangeAttribute_ValidationError";

        public RangoAttribute(Type type, string minimum, string maximum)
            : base(type, minimum, maximum)
        {
            this.ErrorMessageResourceType = typeof(Messages);
            this.ErrorMessageResourceName = RESOURCE_NAME;            
        }
        
        public RangoAttribute(double minimum, double maximum)
            : base(minimum, maximum)
        {
            this.ErrorMessageResourceType = typeof(Messages);
            this.ErrorMessageResourceName = RESOURCE_NAME;
        }
        
        public RangoAttribute(int minimum, int maximum) 
            : base (minimum, maximum)
        {
            this.ErrorMessageResourceType = typeof(Messages);
            this.ErrorMessageResourceName = RESOURCE_NAME;
        }

    }
}