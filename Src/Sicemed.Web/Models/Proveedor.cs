using System.Collections.Generic;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models
{
    public class Proveedor : Entity
    {
        public virtual string RazonSocial { get; set; }
        public virtual Documento Documento { get; set; }
        public virtual Domicilio Domicilio { get; set; }
        public virtual IList<Telefono> Telefonos { get; set; }
    }
}