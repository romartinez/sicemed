using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.Profesional
{
    public class ProfesionalConEspecialidadesViewModel
    {
        public long Id { get; set; }
        public long Documento { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoDocumento { get; set; }
        public List<InfoViewModel> Especialidades { get; set; }

        public ProfesionalConEspecialidadesViewModel()
        {
            Especialidades = new List<InfoViewModel>();
        }
    }
}