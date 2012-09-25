namespace Sicemed.Web.Models.Caja
{
    public class Concepto : Entity
    {
        public virtual string Codigo { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual string CuentaContable { get; set; }
        public virtual TipoConcepto TipoConcepto { get; set; }
        public virtual long Numeracion { get; set; }
    }
}