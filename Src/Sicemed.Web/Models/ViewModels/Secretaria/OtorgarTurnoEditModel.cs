using System;
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
        [SearcheableDropDownProperty(ActionName = "Paciente", ControllerName = "Busqueda", DisplayProperty = "NombreCompleto", Template = "tmplBusquedaPaciente")]
        [Display(Name = "Paciente", Prompt = "Seleccione Paciente")]
        public long? PacienteId { get; set; }

        [Required]
        [UIHint("SearcheableDropDown")]
        [SearcheableDropDownProperty(ActionName = "Profesional", ControllerName = "Busqueda", DisplayProperty = "NombreCompleto", Template = "tmplBusquedaProfesional")]
        [Display(Name = "Profesional", Prompt = "Seleccione Profesional")]
        public long? ProfesionalId { get; set; }

        [Required]
        [Display(Name = "Especialidad")]
        public long? EspecialidadId { get; set; }

        [Required]
        public DateTime FechaTurno { get; set; }

        public long? ConsultorioId { get; set; }

        [HiddenInput]
        public bool EsSobreTurno { get; set; }

        [DisplayName("Es Telefónico?")]
        public bool EsTelefonico { get; set; }

        public OtorgarTurnoEditModel()
        {
            EsTelefonico = true;
            EsSobreTurno = false;
        }
    }
}