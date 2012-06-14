using System;
using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Areas.Admin.Models
{
    public class SearchFiltersViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [SqlDateTimeRange]
        public DateTime Desde { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [SqlDateTimeRange]
        public DateTime Hasta { get; set; }
        
        public string Filtro { get; set; }
    }
}