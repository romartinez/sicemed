using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class DomicilioMap : ComponentMapping<Domicilio>
    {
         public DomicilioMap ()
         {
             Property(x => x.Calle1);
             Property(x => x.Calle2);
             Property(x => x.Localidadad);
         }
    }
}