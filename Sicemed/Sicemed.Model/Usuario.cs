using System;

namespace Sicemed.Model {
    public class Usuario : EntityBase{
        #region Primitive Properties

        public virtual DateTime? FechaNacimiento { get; set; }

        public virtual string TipoDocumento { get; set; }

        public virtual string NumeroDocumento { get; set; }

        #endregion

        #region Navigation Properties

        //public virtual Paciente Paciente { get; set; }

        //public virtual Profesional Profesional { get; set; }

        //public virtual Secretaria Secretaria { get; set; }

        #endregion
    }
}