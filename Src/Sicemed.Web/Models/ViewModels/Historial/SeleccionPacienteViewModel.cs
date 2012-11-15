using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models.ViewModels.Historial
{
    public class SeleccionPacienteViewModel
    {
        [Requerido]
        [UIHint("SearcheableDropDown")]
        [SearcheableDropDownProperty(ActionName = "Paciente", ControllerName = "Busqueda", DisplayProperty = "NombreCompleto", Template = "tmplBusquedaPaciente")]
        [Display(Name = "Paciente", Prompt = "Seleccione Paciente")]
        public long? PacienteId { get; set; }         
    }
}