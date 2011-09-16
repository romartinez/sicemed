using System;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Turno : Entity
    {
        #region Primitive Properties

        public virtual DateTime FechaGeneracion { get; set; }

        public virtual DateTime FechaTurno { get; set; }

        public virtual DateTime? FechaIngreso { get; set; }

        public virtual DateTime? FechaAtencion { get; set; }

        public virtual string Nota { get; set; }

        public virtual string IpPaciente { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Paciente Paciente { get; set; }

        public virtual Profesional Profesional { get; set; }

        public virtual Secretaria Secretaria { get; set; }

        public virtual Especialidad Especialidad { get; set; }

        public virtual Consultorio Consultorio { get; set; }

        #endregion
    }
}