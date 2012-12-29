using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using SICEMED.Web;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Infrastructure.Helpers;

namespace Sicemed.Web.Areas.Admin.Models.Personas
{
    public class AgendaEditModel : IValidatableObject
    {
        [HiddenInput(DisplayValue=false)]
        public virtual long? Id { get; set; }

        [Requerido]
        [DisplayName("Dia")]
        public virtual DayOfWeek Dia { get; set; }

        [Requerido]
        [DisplayName("Duración Turno")]
        [DataType(DataType.Duration)]
        public virtual TimeSpan DuracionTurno { get; set; }

        [Requerido]
        [DisplayName("Horario Desde")]
        [DataType(DataType.Time)]
        public virtual TimeSpan HorarioDesde { get; set; }

        [Requerido]
        [DisplayName("Horario Hasta")]
        [DataType(DataType.Time)]
        public virtual TimeSpan HorarioHasta { get; set; }

        [UIHint("DropDownList")]
        [DisplayName("Consultorio")]
        [DropDownProperty("ConsultorioId")]
        public IEnumerable<SelectListItem> Consultorios { get; set; }

        [Requerido]
        [ScaffoldColumn(false)]
        public long? ConsultorioId { get; set; }

        [UIHint("MultipleList")]
        [DropDownProperty("EspecialidadesSeleccionadas")]
        [DisplayName("Especialidades Atendidas")]
        public IEnumerable<SelectListItem> Especialidades { get; set; }

        [Requerido]
        [ScaffoldColumn(false)]
        public long[] EspecialidadesSeleccionadas { get; set; }

        public AgendaEditModel()
        {
            DuracionTurno = MvcApplication.Clinica.DuracionTurnoPorDefecto;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            var desde = HorarioDesde;
            var hasta = HorarioHasta;

            if (desde >= hasta)
                errors.Add(new ValidationResult("El Horario Desde debe ser menor a Horario Hasta", new[] { "desde" }));

            if (MvcApplication.Clinica.EsHorarioCorrido)
            {
                if (desde < MvcApplication.Clinica.HorarioMatutinoDesde)
                    errors.Add(new ValidationResult(string.Format("El Horario Desde debe ser mayor al Horario de apertura de la clínica ({0}).",
                        MvcApplication.Clinica.HorarioMatutinoDesde.ToString()), new[] { "desde" }));

                if (hasta > MvcApplication.Clinica.HorarioMatutinoHasta)
                    errors.Add(new ValidationResult(string.Format("El Horario Hasta debe ser menor al Horario de cierre de la clínica ({0}).",
                        MvcApplication.Clinica.HorarioMatutinoHasta.ToString()), new[] { "hasta" }));
            }
            else
            {
                //Horario cortado
                if (desde < MvcApplication.Clinica.HorarioMatutinoDesde
                    || (desde > MvcApplication.Clinica.HorarioMatutinoHasta && desde < MvcApplication.Clinica.HorarioVespertinoDesde.Value))
                    errors.Add(new ValidationResult(string.Format("El Horario Desde debe ser mayor al Horario de apertura de la clínica ({0} / {1}).",
                        MvcApplication.Clinica.HorarioMatutinoDesde.ToString(),
                        MvcApplication.Clinica.HorarioVespertinoDesde.Value.ToString()), new[] { "desde" }));

                if (hasta > MvcApplication.Clinica.HorarioVespertinoHasta.Value
                    || (hasta > MvcApplication.Clinica.HorarioMatutinoHasta && hasta < MvcApplication.Clinica.HorarioVespertinoDesde.Value))
                    errors.Add(new ValidationResult(string.Format("El Horario Hasta debe ser menor al Horario de cierre de la clínica ({0}/{1}).",
                        MvcApplication.Clinica.HorarioMatutinoHasta.ToString(),
                        MvcApplication.Clinica.HorarioVespertinoHasta.Value.ToString()), new[] { "desde" }));
            }

            if (hasta.Subtract(desde) < DuracionTurno)            
                errors.Add(new ValidationResult("La duración del turno es inválida, ya que ni un turno puede otorgarse con los horarios de la agenda.", new[] { "DuracionTurno" }));
            
            if(!MvcApplication.Clinica.DiasHabilitados.Contains(Dia))
                errors.Add(new ValidationResult("El día seleccionado para el turno es inválido, la clínica no se encuentra abierta dicho día.", new[] { "Dia" }));

            return errors;
        }
    }
}