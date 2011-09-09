using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models
{
    public class ObraSocial : Entity
    {
        private readonly ISet<Plan> _planes;

        #region Primitive Properties

        [Required]
        public virtual string RazonSocial { get; set; }

        public virtual Documento Documento { get; set; }

        public virtual Domicilio Domicilio { get; set; }

        public virtual Telefono Telefono { get; set; }

        #endregion

        #region Navigation Properties

        public virtual IEnumerable<Plan> Planes
        {
            get { return _planes; }
        }

        #endregion

        public ObraSocial()
        {
            _planes = new HashSet<Plan>();
        }
    }
}