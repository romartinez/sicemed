using DataAnnotationsExtensions;
using Resources;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    public class CorreoAttribute : EmailAttribute
    {
         public CorreoAttribute()
         {
             this.ErrorMessageResourceType = typeof(Messages);
             this.ErrorMessageResourceName = "EmailAttribute_ValidationError";             
         }
    }
}