using System.Collections.Generic;

namespace Sicemed.Model {
    public class Plan : EntityBase {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ObraSocial ObraSocial { get; set; }

        public virtual ISet<Paciente> Pacientes { get; set; }

        #endregion
    }
}