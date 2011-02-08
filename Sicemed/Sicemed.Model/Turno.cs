using System;

namespace Sicemed.Model {
    public class Turno : EntityBase {
        #region Primitive Properties

        public virtual DateTime FechaGeneracion { get; set; }

        public virtual DateTime FechaTurno { get; set; }

        public virtual DateTime? FechaIngreso { get; set; }

        public virtual DateTime? FechaAtencion { get; set; }

        public virtual string Nota { get; set; }

        public virtual string IpPaciente { get; set; }

        #endregion

        #region Navigation Properties

        public virtual DiaAtencionEspecialidadProfesional DiaAtencionEspecialidadProfesional { get; set; }

        public virtual Paciente Paciente { get; set; }

        public virtual Secretaria Secretaria { get; set; }

        #endregion
    }
}