using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models.BI
{
    public class CategoriaIndicador : Entity
    {
        [Requerido]
        public virtual string Nombre { get; set; }
    }
}