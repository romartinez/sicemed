using System;
using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models
{
    public class Feriado : Entity
    {
        #region Primitive Properties

        [Requerido]
        public virtual string Nombre { get; set; }

        [Requerido]
        public virtual DateTime Fecha { get; set; }

        #endregion
    }
}