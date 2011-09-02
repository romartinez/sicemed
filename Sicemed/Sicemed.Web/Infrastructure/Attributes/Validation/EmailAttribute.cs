using System;
using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Infrastructure.Helpers;

namespace Sicemed.Web.Infrastructure.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(ValidationHelper.EMAIL_VALIDATION_REGEX) { }
    }
}