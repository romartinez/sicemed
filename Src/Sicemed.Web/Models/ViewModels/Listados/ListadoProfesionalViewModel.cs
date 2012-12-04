using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.Listados
{
    public class ListadoProfesionalViewModel
    {
        public long? IdEspecialidad { get; set; }
        public string Especialidad { get; set; }
        public IEnumerable<Models.Roles.Profesional> Profesional { get; set; }
    }
}