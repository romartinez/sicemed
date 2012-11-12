using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Infrastructure.Helpers;

namespace Sicemed.Web.Models.ViewModels
{
    public class SearchFiltersViewModel : IValidatableObject
    {
        [Requerido]
        [DataType(DataType.Date)]
        [Fecha]
        public DateTime Desde { get; set; }

        [Requerido]
        [DataType(DataType.Date)]
        [Fecha]
        public DateTime Hasta { get; set; }

        public string Filtro { get; set; }

        public SearchFiltersViewModel()
        {
            Desde = DateTime.Now.AddMonths(-3).StartOfWeek(DayOfWeek.Monday).ToMidnigth();
            Hasta = DateTime.Now.AddDays(1).EndOfWeek(DayOfWeek.Saturday).ToMidnigth();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Desde > Hasta)
                yield return new ValidationResult("La fecha Desde debe ser menor o igual a la fecha Hasta",
                    new[] { "Desde" });
        }
    }
}