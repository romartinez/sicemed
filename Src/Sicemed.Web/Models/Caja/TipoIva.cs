namespace Sicemed.Web.Models.Caja
{
    public class TipoIva : Entity
    {
        public virtual string Descripcion { get; set; }
        public virtual decimal Porcentaje { get; set; }
    }
}