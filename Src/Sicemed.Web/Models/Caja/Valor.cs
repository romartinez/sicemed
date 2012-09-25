using System;

namespace Sicemed.Web.Models.Caja
{
    public abstract class Valor : Entity
    {
        private Moneda _moneda;
        public virtual Moneda Moneda
        {
            get { return _moneda; }
            set
            {
                if (TasaCambioAplicada == default(decimal)) TasaCambioAplicada = value.TasaCambio;
                _moneda = value;
            }
        }

        public virtual Proveedor Proveedor { get; set; }
        public virtual decimal TasaCambioAplicada { get; set; }
        public virtual decimal Importe { get; set; }
        public virtual DateTime FechaCarga { get; set; }
        public virtual DateTime FechaMovimiento { get; set; }
    }

    public class Cheque : Valor
    {
        public virtual int Banco { get; set; }
        public virtual DateTime FechaEmision { get; set; }
        public virtual DateTime FechaVencimiento { get; set; }
        public virtual string NumeroPago { get; set; }
        public virtual TipoCheque TipoCheque { get; set; }
    }

    public class Tarjeta : Valor
    {
        public virtual TipoTarjeta TipoTarjeta { get; set; }
    }

    public class Efectivo : Valor
    {

    }

    public class Transferencia : Valor
    {

    }
}