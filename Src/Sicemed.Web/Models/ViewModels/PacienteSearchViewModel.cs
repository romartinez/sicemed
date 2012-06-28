using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models.ViewModels
{
    public class PacienteSearchViewModel
    {
        [DisplayName("Tipo Documento")]
        [UIHint("DropDownList")]
        [DropDownProperty("TipoDocumento")]
        public IEnumerable<SelectListItem> TipoDocumentosHabilitados { get; set; }

        [ScaffoldColumn(false)]
        public int? TipoDocumento { get; set; }

        [DisplayName("Número Documento")]
        public long? NumeroDocumento { get; set; }

        public string Nombre { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<InfoViewModel> PacientesEncontrados { get; set; }

        [ScaffoldColumn(false)]
        public InfoViewModel PacienteSeleccionado { get; set; }

        [ScaffoldColumn(false)]
        public bool BusquedaEfectuada { get; set; }

        [ScaffoldColumn(false)]
        public bool HayPacienteSeleccionado
        {
            get
            {
                return PacienteSeleccionado != null;
            }
        }

        public PacienteSearchViewModel()
        {
            PacientesEncontrados = new List<InfoViewModel>();
            BusquedaEfectuada = false;
        }
    }
}