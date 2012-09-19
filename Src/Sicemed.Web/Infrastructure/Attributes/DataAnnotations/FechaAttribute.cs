using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using Resources;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    public class FechaAttribute : ValidationAttribute, IClientValidatable, IMetadataAware
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }

        public FechaAttribute() : this("01/01/2012", "01/01/3000") { }

        public FechaAttribute(string minDate, string maxDate)
        {
            MinDate = ParseDate(minDate);
            MaxDate = ParseDate(maxDate);
            ErrorMessageResourceType = typeof(Messages);
            ErrorMessageResourceName = "FechaAttribute_ValidationError";
        }

        public override bool IsValid(object value)
        {
            if (value == null || !(value is DateTime))
            {
                return true;
            }
            var dateValue = (DateTime)value;
            return MinDate <= dateValue && dateValue <= MaxDate;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MinDate, MaxDate);
        }

        private static DateTime ParseDate(string dateValue)
        {
            return DateTime.Parse(dateValue, CultureInfo.CurrentCulture);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] {
            new ModelClientValidationRangeDateRule(  FormatErrorMessage(metadata.GetDisplayName()), 
                MinDate, MaxDate)
            };
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.DataTypeName = "Date";
        }
    }

    public class ModelClientValidationRangeDateRule : ModelClientValidationRule
    {
        public ModelClientValidationRangeDateRule(string errorMessage, DateTime minValue, DateTime maxValue)
        {
            ErrorMessage = errorMessage;
            ValidationType = "rangedate";

            ValidationParameters["min"] = minValue.ToString("dd/MM/yyyy");
            ValidationParameters["max"] = maxValue.ToString("dd/MM/yyyy");
        }
    }
}