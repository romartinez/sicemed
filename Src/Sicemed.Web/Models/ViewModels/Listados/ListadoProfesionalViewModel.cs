using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.Listados
{
    public class ListadoProfesionalViewModel
    {
        public string Nombre { get; set; }
        public List<string> Especialidades { get; set; }
    }
}