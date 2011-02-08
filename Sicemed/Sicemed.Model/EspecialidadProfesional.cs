using System.Collections.Generic;

namespace Sicemed.Model {
    public class EspecialidadProfesional : EntityBase {
        #region Navigation Properties

        public virtual ISet<DiaAtencionEspecialidadProfesional> DiaAtencionEspecialidad { get; set; }

        public virtual Especialidad Especialidad { get; set; }

        public virtual Profesional Profesional { get; set; }

        #endregion
    }
}