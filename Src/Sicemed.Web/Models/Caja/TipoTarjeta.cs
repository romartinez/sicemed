namespace Sicemed.Web.Models.Caja
{
    public class TipoTarjeta
    {
        public virtual string Nombre { get; set; }
        public virtual string CuentaContable { get; set; }
        public virtual bool Habilitada { get; set; }
        public virtual ModoTarjeta Modo { get; set; }
    }
}