using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models
{
    public class Calendario : EntityBase
    {
        #region Primitive Properties

        public virtual DateTime Anio { get; set; }

        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<Feriado> Feriados { get; set; }

        #endregion
    }
}