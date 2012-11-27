using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models.ViewModels.Secretaria
{
    public class EdicionObraSocialTurnoEditModel
    {
        [HiddenInput]
        public long? Id { get; set; }

        [UIHint("DropDownList")]
        [DisplayName("Obra Social")]
        [DropDownProperty("ObraSocialId")]
        public IEnumerable<SelectListItem> ObrasSocialesHabilitadas { get; set; }

        [DisplayName("Plan")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownProperty("PlanId", "ObraSocialId", "GetPlanesObraSocial", "Domain", "Admin", "obraSocialId", "<< Seleccione una Obra Social >>")]
        public IEnumerable<SelectListItem> PlanesObraSocialHabilitados { get; set; }

        [ScaffoldColumn(false)]
        public virtual long? PlanId { get; set; }

        [ScaffoldColumn(false)]
        public virtual long? ObraSocialId { get; set; }

        [DisplayName("Número Afiliado")]
        [LargoCadenaPorDefecto]
        public virtual string NumeroAfiliado { get; set; }

    }
}