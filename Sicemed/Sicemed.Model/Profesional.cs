using System.Collections.Generic;

namespace Sicemed.Model {
    public class Profesional : EntityBase {
        #region Primitive Properties

        public virtual string Domicilio { get; set; }

        public virtual string CUIT_CUIL { get; set; }

        public virtual string NumeroMatricula { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<EspecialidadProfesional> EspecialidadProfesional { get; set; }

        public virtual Localidad Localidad { get; set; }

        //public virtual Usuario Usuario { get; set; }

        #endregion
    }
}