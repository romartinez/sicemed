using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sicemed.Web.Models.Components
{
    public class Telefono : ComponentBase
    {
        public virtual string Numero { get; set; }
        public virtual string Prefijo { get; set; }
    }
}