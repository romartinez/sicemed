using System;

namespace Sicemed.Web.Models.Caja
{
    public abstract class Factura : Entity
    {
        public virtual TipoFactura TipoFactura { get; set; }
        public virtual string NumeroFactura { get; set; }
        public virtual TipoIva TipoIva { get; set; }
        public virtual decimal ImporteIva { get; set; }
        public virtual decimal ImporteIngresosBrutos { get; set; }
        public virtual decimal Ganancias { get; set; }
        public virtual decimal OtrasRetenciones { get; set; }
        public virtual decimal ImporteTotal { get; set; }
        public virtual DateTime FechaFactura { get; set; }
        public virtual DateTime FechaCarga { get; set; }
        public virtual string Comentarios { get; set; }
        public virtual EstadoFactura Estado { get; set; }
        public virtual Persona Operador { get; set; }
        public virtual decimal Saldo { get; set; }
    }

    public class FacturaRecibida : Factura
    {
        public virtual Proveedor Proveedor { get; set; }
    }

    public class FacturaEmitida : Factura
    {
    }
}