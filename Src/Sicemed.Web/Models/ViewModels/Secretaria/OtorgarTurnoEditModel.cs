using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models.ViewModels.Secretaria
{
    public class OtorgarTurnoEditModel
    {
        [Requerido]
        [UIHint("SearcheableDropDown")]
        [SearcheableDropDownProperty(ActionName = "Paciente", ControllerName = "Busqueda", DisplayProperty = "NombreCompleto", Template = "tmplBusquedaPaciente")]
        [Display(Name = "Paciente", Prompt = "Seleccione Paciente")]
        public long? PacienteId { get; set; }

        [Requerido]
        [UIHint("SearcheableDropDown")]
        [SearcheableDropDownProperty(ActionName = "Profesional", ControllerName = "Busqueda", DisplayProperty = "NombreCompleto", Template = "tmplBusquedaProfesional")]
        [Display(Name = "Profesional", Prompt = "Seleccione Profesional")]
        public long? ProfesionalId { get; set; }

        [Requerido]
        [Display(Name = "Especialidad")]
        public long? EspecialidadId { get; set; }

        [Requerido]
        public DateTime FechaTurno { get; set; }

        public long? ConsultorioId { get; set; }

        [HiddenInput]
        public bool EsSobreTurno { get; set; }

        [Requerido]
        [DisplayName("Duración Turno")]
        [HiddenInput]
        [Rango(typeof(TimeSpan), "00:00:05", "36:00:00")]
        public TimeSpan DuracionTurno { get; set; }
        
        [DisplayName("Es Telefónico?")]
        public bool EsTelefonico { get; set; }

        public OtorgarTurnoEditModel()
        {
            EsTelefonico = true;
            EsSobreTurno = false;
        }
    }
}