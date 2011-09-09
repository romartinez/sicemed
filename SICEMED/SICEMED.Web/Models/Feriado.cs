using System;
using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Models
{
    public class Feriado : Entity
    {
        #region Primitive Properties

        [Required]
        public virtual string Nombre { get; set; }

        [Required]
        public virtual DateTime Fecha { get; set; }

        #endregion
    }
}