using System.ComponentModel.DataAnnotations;
using Resources;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    public class RequeridoAttribute : RequiredAttribute
    {         
        public RequeridoAttribute()
        {
            this.ErrorMessageResourceType = typeof (Messages);
            this.ErrorMessageResourceName = "RequiredAttribute_ValidationError";
        }
    }
}