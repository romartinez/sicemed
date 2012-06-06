using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Areas.Admin.Models.Personas
{
    public class AgendaEditModel
    {
        [HiddenInput]
        public virtual long? Id { get; set; }

        [Required]
        [DisplayName("Dia")]
        public virtual DayOfWeek Dia { get; set; }

        [Required]
        [DisplayName("Duración Turno")]
        [DataType(DataType.Duration)]
        public virtual TimeSpan DuracionTurno { get; set; }

        [Required]
        [DisplayName("Horario Desde")]
        [DataType(DataType.Time)]
        public virtual DateTime HorarioDesde { get; set; }

        [Required]
        [DisplayName("Horario Hasta")]
        [DataType(DataType.Time)]
        public virtual DateTime HorarioHasta { get; set; }

        [UIHint("DropDownList")]
        [DisplayName("Consultorio")]
        [DropDownProperty("ConsultorioId")]
        public IEnumerable<SelectListItem> Consultorios { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public long? ConsultorioId { get; set; }

        [UIHint("MultipleList")]
        [DropDownProperty("EspecialidadesSeleccionadas")]
        [DisplayName("Especialidades Atendidas")]        
        public IEnumerable<SelectListItem> Especialidades { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public long[] EspecialidadesSeleccionadas { get; set; }
    }
}