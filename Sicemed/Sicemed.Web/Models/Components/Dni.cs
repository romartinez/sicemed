namespace Sicemed.Web.Models.Components
{
    public class Dni : Documento
    {
        public override string Descripcion
        {
            get { return "DNI"; }
        }

        public override string DescripcionCorta
        {
            get { return "Documento Nacional De Identidad"; }
        }
    }
}