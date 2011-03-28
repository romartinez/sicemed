using System;
using System.Web.Script.Serialization;

namespace Sicemed.Web.Models
{
    public class Feriado : EntityBase
    {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        public virtual DateTime? FechaAjustada { get; set; }

        public virtual DateTime FechaOriginal { get; set; }

        #endregion

        #region Navigation Properties
        [ScriptIgnore]
        public virtual Calendario Calendario { get; set; }

        #endregion
    }
}