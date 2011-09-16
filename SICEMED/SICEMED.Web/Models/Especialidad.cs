using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Models
{
    public class Especialidad : Entity
    {
        #region Primitive Properties

        [Required]
        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        #endregion
    }
}