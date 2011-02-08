using System.Collections.Generic;

namespace Sicemed.Model {
    public class Provincia : EntityBase {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<Localidad> Localidades { get; set; }

        #endregion
    }
}