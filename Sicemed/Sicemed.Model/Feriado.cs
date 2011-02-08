using System;

namespace Sicemed.Model {
    public class Feriado : EntityBase {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        public virtual DateTime? FechaAjustada { get; set; }

        public virtual DateTime FechaOriginal { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Calendario Calendario { get; set; }

        #endregion
    }
}