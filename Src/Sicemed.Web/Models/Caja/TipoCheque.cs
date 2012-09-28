namespace Sicemed.Web.Models.Caja
{
    public class TipoCheque : Entity
    {
        public virtual string Nombre { get; set; }
        public virtual string CodigoBancoInicial { get; set; }
        public virtual string CuentaContable { get; set; }
    }
}