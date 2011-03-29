namespace Sicemed.Web.Models.Components.Documentos
{
    public class Cuil : Documento
    {
        public override string Descripcion
        {
            get { return "C.U.I.L."; }
        }

        public override string DescripcionCorta
        {
            get { return "Código Único de Identificación Laboral"; }
        }
    }
}