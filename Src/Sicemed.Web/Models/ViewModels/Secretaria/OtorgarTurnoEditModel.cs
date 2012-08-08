using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models.ViewModels.Secretaria
{
    public class OtorgarTurnoEditModel
    {
        [Required]
        [UIHint("SearcheableDropDown")]
        [SearcheableDropDownProperty(ActionName= "Paciente", ControllerName = "Busqueda", DisplayProperty = "NombreCompleto", Template = "tmplBusquedaPaciente")]
        [Display(Name = "Paciente", Prompt = "Seleccione Paciente")]
        public long? PacienteId { get; set; }

        [Required]
        [UIHint("SearcheableDropDown")]
        [SearcheableDropDownProperty(ActionName = "Profesional", ControllerName = "Busqueda", DisplayProperty = "NombreCompleto", Template = "tmplBusquedaProfesional")]
        [Display(Name = "Profesional", Prompt = "Seleccione Profesional")]
        public long? ProfesionalId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public string EspecialidadId { get; set; }

        [DisplayName("Especialidad")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownPropertyAttribute("EspecialidadId", "ProfesionalId", "GetEspecialidadesProfesional", "Secretaria", "", "profesioanlId", "<< Seleccione un Profesional >>")]
        public IEnumerable<SelectListItem> EspecialidadesProfesional { get; set; }
        
        [Required]
        [ScaffoldColumn(false)]
        public string TurnoId { get; set; }

        [DisplayName("Turno")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownPropertyAttribute("TurnoId", "EspecialidadId", "GetTurnosDisponiblesProfesional", "Secretaria", "", "especialidadId", "<< Seleccione una Especialidad >>")]
        public IEnumerable<SelectListItem> TurnosDisponibles { get; set; }

        [DisplayName("Es Telefónico?")]
        public bool EsTelefonico { get; set; }
    }
}