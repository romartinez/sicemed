using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models
{
    public class Plan : Entity
    {
        #region Primitive Properties

        [Requerido]
        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        public virtual decimal Coseguro { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ObraSocial ObraSocial { get; set; }

        #endregion

      
    }
}