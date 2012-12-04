using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.Listados
{
    public class ListadoObraSocialesViewModel
    {
        public string Nombre { get; set; }
        public IEnumerable<Plan> Planes { get; set; }
    }
}