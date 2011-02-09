using System;
using System.Collections.Generic;

namespace Sicemed.Model {
    public class Secretaria : EntityBase {
        #region Primitive Properties

        public virtual string CUIT_CUIL { get; set; }


        public virtual DateTime? FechaIngreso { get; set; }

        public virtual string Domicilio { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Localidad Localidad { get; set; }

        //public virtual Usuario Usuario { get; set; }

        public virtual ISet<Turno> Turnos { get; set; }

        #endregion
    }
}