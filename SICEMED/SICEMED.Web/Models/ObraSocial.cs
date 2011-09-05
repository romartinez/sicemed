


using System.Collections.Generic;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models
{
    public class ObraSocial : Entity
    {
        #region Primitive Properties

        public virtual string RazonSocial { get; set; }

        public virtual Documento Documento { get; set; }

        public virtual Domicilio Direccion { get; set; }

        public virtual Telefono Telefono { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<Plan> Planes { get; set; }

        #endregion
    }
}