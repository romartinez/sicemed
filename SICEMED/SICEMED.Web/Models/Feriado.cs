using System;

namespace Sicemed.Web.Models
{
    public class Feriado : Entity
    {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        public virtual DateTime? FechaAjustada { get; set; }

        public virtual DateTime FechaOriginal { get; set; }

        #endregion
    }
}