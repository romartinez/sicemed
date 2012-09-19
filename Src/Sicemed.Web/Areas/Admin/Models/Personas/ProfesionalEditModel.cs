using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Areas.Admin.Models.Personas
{
    [DisplayName("profesional")]
    public class ProfesionalEditModel
    {
        [HiddenInput]
        public virtual long? Id { get; set; }

        [Requerido]
        [DisplayName("Matricula")]
        [LargoCadenaPorDefecto]
        public virtual string Matricula { get; set; }

        [DisplayName("Retención Fija")]        
        public virtual string RetencionFija { get; set; }

        [DisplayName("Retención Porcentaje")]        
        public virtual string RetencionPorcentaje { get; set; }
        
        [UIHint("Agendas")]
        [DisplayName("Dias Atención")]
        public virtual IEnumerable<AgendaEditModel> Agendas { get; set; }

        [UIHint("MultipleList")]
        [DisplayName("Especialidades Atendidas")]
        [DropDownProperty("EspecialidadesSeleccionadas")]
        public IEnumerable<SelectListItem> Especialidades { get; set; }

        [Requerido]
        [ScaffoldColumn(false)]
        public long[] EspecialidadesSeleccionadas { get; set; }
    }
}