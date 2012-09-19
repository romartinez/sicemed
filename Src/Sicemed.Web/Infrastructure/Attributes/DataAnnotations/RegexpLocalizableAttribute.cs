using System.ComponentModel.DataAnnotations;
using Resources;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    public class ExpresionRegularAttribute : RegularExpressionAttribute
    {
        public ExpresionRegularAttribute(string pattern)
            : base(pattern)
        {
            this.ErrorMessageResourceType = typeof(Messages);
            this.ErrorMessageResourceName = "RegexAttribute_ValidationError";
        }
    }
}