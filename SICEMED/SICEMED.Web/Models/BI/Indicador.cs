using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Models.BI.Enumerations;

namespace Sicemed.Web.Models.BI
{
    public class Indicador : Entity
    {
        [Required]
        public virtual CategoriaIndicador Categoria { get; set; }
        [Required]
        public virtual string Nombre { get; set; }
        public virtual bool Habilitado { get; set; }
        [Required]
        public virtual string Codigo { get; set; }
        [Required]
        public virtual TipoOperadorIndicador TipoOperador { get; set; }
    }
}