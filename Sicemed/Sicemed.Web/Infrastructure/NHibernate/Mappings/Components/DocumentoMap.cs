using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations.Documentos;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class DocumentoMap : ComponentMapping<Documento>
    {
        public DocumentoMap()
         {
             Property(x => x.TipoDocumento, map=>map.Type<EnumerationType<TipoDocumento>>());             
             Property(x => x.Numero);
         }
    }
}