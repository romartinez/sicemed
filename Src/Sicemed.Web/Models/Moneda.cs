namespace Sicemed.Web.Models
{
    public class Moneda : Entity
    {
        public virtual string Simbolo { get; set; }
        public virtual string Nombre { get; set; }
        public virtual decimal TasaCambio { get; set; }
    }
}