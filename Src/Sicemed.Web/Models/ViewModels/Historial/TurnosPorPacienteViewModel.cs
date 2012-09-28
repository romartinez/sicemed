using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Infrastructure.Helpers;

namespace Sicemed.Web.Models.ViewModels.Historial
{
    public class TurnosPorPacienteViewModel : IValidatableObject
    {
        public bool BusquedaEfectuada { get; set; }

        [Requerido]
        [DataType(DataType.Date)]
        [Fecha]
        public DateTime Desde { get; set; }

        [Requerido]
        [DataType(DataType.Date)]
        [Fecha]
        public DateTime Hasta { get; set; }

        public string Filtro { get; set; }

        [Requerido]
        [UIHint("SearcheableDropDown")]
        [SearcheableDropDownProperty(ActionName = "Paciente", ControllerName = "Busqueda", DisplayProperty = "NombreCompleto", Template = "tmplBusquedaPaciente")]
        [Display(Name = "Paciente", Prompt = "Seleccione Paciente")]
        public long? PacienteId { get; set; }

        public IEnumerable<HistorialItem> Turnos { get; set; }

        public TurnosPorPacienteViewModel()
        {
            BusquedaEfectuada = false;
            Turnos = new List<HistorialItem>();
            Desde = DateTime.Now.AddMonths(-3).ToMidnigth();
            Hasta = DateTime.Now.AddDays(1).ToMidnigth();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Desde > Hasta)
                yield return new ValidationResult("La fecha Desde debe ser menor o igual a la fecha Hasta",
                    new[] { "Desde" });
        }

        public class HistorialItem
        {
            public DateTime FechaTurno { get; set; }
            public InfoViewModel Profesional { get; set; }
            public InfoViewModel CanceladoPor { get; set; }
            public DateTime? FechaCancelacion { get; set; }
            public string MotivoCancelacion { get; set; }
            public InfoViewModel Especialidad { get; set; }
            public InfoViewModel Consultorio { get; set; }
            public string Nota { get; set; }
            public Turno.EstadoTurno Estado { get; protected set; }
            public DateTime FechaEstado { get; protected set; }
            public bool PuedoCancelar { get; set; }
            public bool PuedoPresentar { get; set; }
            public bool PuedoAtender { get; set; }
            public bool EsObtenidoWeb { get; protected set; }
            public bool EsObtenidoPersonalmente { get; protected set; }
            public bool EsObtenidoTelefonicamente { get; protected set; }
        }
    }
}