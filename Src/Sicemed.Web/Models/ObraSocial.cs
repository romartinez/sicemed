using System.ComponentModel.DataAnnotations;
using Iesi.Collections.Generic;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models
{
    public class ObraSocial : Entity
    {
        private readonly ISet<Plan> _planes;

        #region Primitive Properties

        [Requerido]
        public virtual string RazonSocial { get; set; }

        public virtual Documento Documento { get; set; }

        public virtual Domicilio Domicilio { get; set; }

        public virtual Telefono Telefono { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<Plan> Planes
        {
            get { return _planes; }
        }

        #endregion

        public ObraSocial()
        {
            _planes = new HashedSet<Plan>();
        }
    }
}