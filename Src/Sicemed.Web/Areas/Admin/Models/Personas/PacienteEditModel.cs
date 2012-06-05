using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Areas.Admin.Models.Personas
{
    [DisplayName("paciente")]
    public class PacienteEditModel
    {
        [UIHint("DropDownList")]
        [DisplayName("Obra Social")]
        [DropDownProperty("ObraSocialId")]
        public IEnumerable<SelectListItem> ObrasSocialesHabilitadas { get; set; }

        [DisplayName("Plan")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownProperty("PlanId", "ObraSocialId", "GetPlanesObraSocial", "Domain", "Admin", "obraSocialId", "<< Seleccione una Obra Social >>")]
        public IEnumerable<SelectListItem> PlanesObraSocialHabilitados { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public virtual long? PlanId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public virtual long? ObraSocialId { get; set; }

        [Required]
        [DisplayName("Número Afiliado")]
        [DefaultStringLength]
        public virtual string NumeroAfiliado { get; set; }

        [DisplayName("Habilitado Turnos Web?")]
        public virtual bool EstaHabilitadoTurnosWeb { get; set; }

        [DisplayName("Inasistencias Continuas")]
        public virtual int InasistenciasContinuas { get; set; }
    }
}