using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Components.Documentos;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class DocumentoMap : ComponentMapping<Documento>
    {
        public DocumentoMap()
         {
             Property(x => x.Descripcion);
             Property(x => x.DescripcionCorta);
             Property(x => x.Numero);
         }
    }
}