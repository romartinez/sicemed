using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using Resources;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    public class LargoCadenaMinimoAttribute : MinLengthAttribute
    {
        public LargoCadenaMinimoAttribute(int length)
            : base(length)
         {
             this.ErrorMessageResourceType = typeof(Messages);
             this.ErrorMessageResourceName = "MinLengthAttribute_ValidationError";             
         }
    }
}