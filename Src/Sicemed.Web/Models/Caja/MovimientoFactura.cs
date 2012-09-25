using System;

namespace Sicemed.Web.Models.Caja
{
    public class MovimientoFactura : Entity
    {
        public virtual decimal Importe { get; set; }
        public virtual Valor Valor { get; set; }
        public virtual DateTime FechaCarga { get; set; }
    }
}