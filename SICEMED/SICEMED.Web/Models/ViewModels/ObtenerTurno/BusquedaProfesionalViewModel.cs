using System.Collections.Generic;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models.ViewModels.ObtenerTurno
{
    public class BusquedaProfesionalViewModel
    {
        public virtual IEnumerable<Especialidad> Especialidades { get; set; }

        public virtual long EspecialidadId { get; set; }
        public virtual string Profesional { get; set; }

        public IEnumerable<Profesional> ProfesionalesEncontrados { get; set; }
    }
}