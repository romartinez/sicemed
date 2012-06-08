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
        [ScaffoldColumn(false)]
        public long? PacienteId { get; set; }

        [DisplayName("Paciente")]
        [UIHint("DropDownList")]
        [DropDownProperty("PacienteId")]
        public IEnumerable<SelectListItem> PacientesDisponibles { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public long? ProfesionalId { get; set; }

        [DisplayName("Profesional")]
        [UIHint("DropDownList")]
        [DropDownProperty("ProfesionalId")]
        public IEnumerable<SelectListItem> ProfesionalesDisponibles { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public string EspecialidadId { get; set; }

        [DisplayName("Especialidad")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownPropertyAttribute("EspecialidadId", "ProfesionalId", "GetEspecialidadesProfesional", "Secretaria", "", "profesioanlId", "<< Seleccione un Profesional >>")]
        public IEnumerable<SelectListItem> EspecialidadesProfesional { get; set; }
        
        [Required]
        [ScaffoldColumn(false)]
        public long? TurnoId { get; set; }

        [DisplayName("Turno")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownPropertyAttribute("TurnoId", "EspecialidadId", "GetTurnosDisponiblesProfesional", "Secretaria", "", "especialidadId", "<< Seleccione una Especialidad >>")]
        public IEnumerable<SelectListItem> TurnosDisponibles { get; set; }

        [DisplayName("Es Telefónico?")]
        public bool EsTelefonico { get; set; }
    }
}