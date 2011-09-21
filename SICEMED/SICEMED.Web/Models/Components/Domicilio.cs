namespace Sicemed.Web.Models.Components
{
    public class Domicilio : ComponentBase
    {
        public virtual string Direccion { get; set; }
        public virtual Localidad Localidad { get; set; }

        public virtual double Latitud { get; set; }
        public virtual double Longitud { get; set; }
    }
}