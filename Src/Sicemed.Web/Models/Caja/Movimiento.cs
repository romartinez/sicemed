using System;
using Iesi.Collections.Generic;

namespace Sicemed.Web.Models.Caja
{
    public class Movimiento : Entity
    {
        public virtual Caja Caja { get; set; }
        public virtual DateTime FechaMovimiento { get; set; }
        public virtual ISet<MovimientoFactura> Items { get; set; }
    }
}