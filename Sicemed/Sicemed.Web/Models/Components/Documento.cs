using Sicemed.Web.Models.Enumerations.Documentos;

namespace Sicemed.Web.Models.Components
{
    public class Documento : ComponentBase
    {
        public virtual TipoDocumento TipoDocumento{ get; set; }
        public virtual long Numero { get; set; }
    }
}