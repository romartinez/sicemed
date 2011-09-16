using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class ValidationHelper
    {
        public const string EMAIL_VALIDATION_REGEX =
            @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b";

        public static IEnumerable<ValidationResult> Validate(object component)
        {
            return from descriptor in TypeDescriptor.GetProperties(component).Cast<PropertyDescriptor>()
                   from validation in descriptor.Attributes.OfType<ValidationAttribute>()
                   where !validation.IsValid(descriptor.GetValue(component))
                   select new ValidationResult(
                       validation.FormatErrorMessage(validation.ErrorMessage) ??
                       string.Format(CultureInfo.CurrentUICulture, "{0} validation failed.", validation.GetType().Name),
                       new[] {descriptor.Name});
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email");

            return Regex.IsMatch(email, EMAIL_VALIDATION_REGEX);
        }
    }
}