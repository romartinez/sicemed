using System.Collections.Generic;

namespace Sicemed.Web.Models {
    public class EspecialidadProfesional : EntityBase {
        #region Navigation Properties

        public virtual ISet<DiaAtencionEspecialidadProfesional> DiaAtencionEspecialidad { get; set; }

        public virtual Especialidad Especialidad { get; set; }

        public virtual Usuario Profesional { get; set; }

        #endregion
    }
}