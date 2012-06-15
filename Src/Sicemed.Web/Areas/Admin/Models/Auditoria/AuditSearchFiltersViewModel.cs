using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Areas.Admin.Models.Auditoria
{
    public class AuditSearchFiltersViewModel : SearchFiltersViewModel
    {
        [UIHint("DropDownList")]
        [DisplayName("Accion")]
        [DropDownProperty("Accion")]
        public IEnumerable<SelectListItem> AccionesHabilitadas { get; set; }

        [ScaffoldColumn(false)]
        public string Accion { get; set; }

        public string Usuario { get; set; }

        [DisplayName("Id Entidad")]
        public long? EntidadId { get; set; }
        
        public string Entidad { get; set; }

        public AuditSearchFiltersViewModel()
        {
           AccionesHabilitadas = new[]
            {
                new SelectListItem() {Text = "INSERT"},
                new SelectListItem() {Text = "UPDATE"},
                new SelectListItem() {Text = "DELETE"},
            };
        }
    }
}