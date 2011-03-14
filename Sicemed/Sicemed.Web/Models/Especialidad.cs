using System.Collections.Generic;

namespace Sicemed.Web.Models {
    public class Especialidad : EntityBase {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<EspecialidadProfesional> EspecialidadProfesionales { get; set; }

        #endregion
    }
}