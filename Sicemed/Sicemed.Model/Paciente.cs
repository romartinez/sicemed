using System.Collections.Generic;

namespace Sicemed.Model {
    public class Paciente : EntityBase {
        #region Primitive Properties

        public virtual bool? EstaHabilitadoTurnosWeb { get; set; }

        public virtual string Telefono { get; set; }

        public virtual int InasistenciasContinuas { get; set; }

        public virtual string NumeroAfiliado { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Localidad Localidad { get; set; }

        public virtual Plan Plan { get; set; }

        //public virtual Usuario Usuario { get; set; }

        public virtual ISet<Turno> Turnos { get; set; }

        #endregion
    }
}