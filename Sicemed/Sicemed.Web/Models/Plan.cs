using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Models
{
    public class Plan : Entity
    {
        #region Primitive Properties
        [Required]
        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ObraSocial ObraSocial { get; set; }

        #endregion
    }
}