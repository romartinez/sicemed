namespace Sicemed.Web.Models.Caja
{
    public class Caja : Entity
    {
        public virtual Persona Operador { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual decimal MontoMinimo { get; set; }
    }
}