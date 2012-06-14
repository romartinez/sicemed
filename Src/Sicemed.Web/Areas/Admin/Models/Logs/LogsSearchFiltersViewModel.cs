using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Areas.Admin.Models.Logs
{
    public class LogsSearchFiltersViewModel : SearchFiltersViewModel
    {
        [UIHint("DropDownList")]
        [DisplayName("Nivel Log")]
        [DropDownProperty("LogLevelSeleccionado")]
        public IEnumerable<SelectListItem> LogLevelsHabilitados { get; set; }
        
        [ScaffoldColumn(false)]
        public string  LogLevelSeleccionado { get; set; }

        public LogsSearchFiltersViewModel()
        {
            LogLevelsHabilitados = new[]
            {
                new SelectListItem() {Text = "FATAL"},
                new SelectListItem() {Text = "ERROR"},
                new SelectListItem() {Text = "WARN"},
                new SelectListItem() {Text = "INFO"},
                new SelectListItem() {Text = "DEBUG"}
            };
        }
    }
}