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
        [Required]
        [DisplayName("Matricula")]
        [DefaultStringLength]
        public virtual string Matricula { get; set; }
        
        public virtual IEnumerable<AgendaEditViewModel> Agendas { get; set; }

        [UIHint("MultipleList")]
        [DisplayName("Especialidades Atendidas")]
        public IEnumerable<SelectListItem> Especialidades { get; set; }
    }
}