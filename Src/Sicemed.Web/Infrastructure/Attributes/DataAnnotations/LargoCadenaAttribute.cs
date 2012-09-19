using System.ComponentModel.DataAnnotations;
using Resources;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    public class LargoCadenaAttribute : StringLengthAttribute
    {
        public LargoCadenaAttribute(int maximumLength)
            : base(maximumLength)
        {
            this.ErrorMessageResourceType = typeof(Messages);
            this.ErrorMessageResourceName = this.MinimumLength == 0 ? 
                "StringLengthAttribute_ValidationError" : "StringLengthAttribute_ValidationErrorIncludingMinimum";
        }
    }
}