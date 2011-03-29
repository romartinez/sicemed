using System;
using Iesi.Collections.Generic;

namespace Sicemed.Web.Models
{
    public class Usuario : Entity
    {
        #region Primitive Properties

        public virtual DateTime? FechaNacimiento { get; set; }

        //public virtual Documento Documento { get; set; }

        public virtual DateTime? FechaIngreso { get; set; }

        public virtual string Domicilio { get; set; }

        public virtual string NumeroMatricula { get; set; }

        public virtual bool? EstaHabilitadoTurnosWeb { get; set; }

        public virtual string Telefono { get; set; }

        public virtual int InasistenciasContinuas { get; set; }

        public virtual string NumeroAfiliado { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Localidad Localidad { get; set; }

        public virtual ISet<Turno> Turnos { get; set; }

        public virtual ISet<EspecialidadProfesional> EspecialidadProfesional { get; set; }

        public virtual Plan Plan { get; set; }

        #endregion
    }
}