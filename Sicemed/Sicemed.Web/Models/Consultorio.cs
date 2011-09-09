using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Models
{
    public class Consultorio : Entity
    {
        #region Primitive Properties

        [Required]
        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        #endregion
    }
}