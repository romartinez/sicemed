using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sicemed.Web.Models.Components
{
    public class Domicilio : ComponentBase
    {
        public virtual string Calle1 { get; set; }
        public virtual string Calle2 { get; set; }
        public virtual Localidad Localidadad { get; set; }
    }
}