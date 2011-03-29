
using Iesi.Collections.Generic;

namespace Sicemed.Web.Models
{
    public class EspecialidadProfesional : Entity
    {
        #region Navigation Properties

        public virtual ISet<DiaAtencionEspecialidadProfesional> DiaAtencionEspecialidad { get; set; }

        public virtual Especialidad Especialidad { get; set; }

        public virtual Usuario Profesional { get; set; }

        #endregion
    }
}