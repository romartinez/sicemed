using System.Collections.Generic;

namespace Sicemed.Web.Models {
    public class ObraSocial : EntityBase {
        #region Primitive Properties

        public virtual string RazonSocial { get; set; }

        public virtual string CUIT { get; set; }

        public virtual string Direccion { get; set; }

        public virtual string Telefono { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Localidad Localidad { get; set; }

        public virtual ISet<Plan> Planes { get; set; }

        #endregion
    }
}