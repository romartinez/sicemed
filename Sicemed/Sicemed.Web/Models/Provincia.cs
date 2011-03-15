
using Iesi.Collections.Generic;

namespace Sicemed.Web.Models
{
    public class Provincia : EntityBase
    {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<Localidad> Localidades { get; set; }

        #endregion
    }
}