using System;
using Iesi.Collections.Generic;

namespace Sicemed.Web.Models.Caja
{
    public class MovimientoFactura : Entity
    {
        public virtual decimal Importe { get; set; }
        public virtual Factura Factura { get; set; }
        public virtual DateTime FechaCarga { get; set; }
        public virtual ISet<Valor> Valores { get; set; }
    }
}