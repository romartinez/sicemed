using System.Collections.Generic;

namespace Sicemed.Web.Models {
    public class Plan : EntityBase {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ObraSocial ObraSocial { get; set; }

        public virtual ISet<Usuario> Pacientes { get; set; }

        #endregion
    }
}