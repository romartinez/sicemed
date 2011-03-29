namespace Sicemed.Web.Models.Components.Documentos
{
    public class Cuit : Documento
    {
        public override string Descripcion
        {
            get { return "C.U.I.T."; }
        }

        public override string DescripcionCorta
        {
            get { return "Código Único de Identificación Tributario"; }
        }
    }
}