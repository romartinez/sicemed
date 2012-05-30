using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Models.BI
{
    public class CategoriaIndicador : Entity
    {
        [Required]
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
    }
}