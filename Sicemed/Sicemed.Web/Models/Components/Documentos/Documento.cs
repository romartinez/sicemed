namespace Sicemed.Web.Models.Components.Documentos
{
    public abstract class Documento : ComponentBase
    {
        public abstract string Descripcion { get; }
        public abstract string DescripcionCorta { get; }
        public virtual long Numero { get; set; }
    }
}