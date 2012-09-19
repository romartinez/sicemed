using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models
{
    public class Consultorio : Entity
    {
        #region Primitive Properties

        [Requerido]
        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        #endregion
    }
}