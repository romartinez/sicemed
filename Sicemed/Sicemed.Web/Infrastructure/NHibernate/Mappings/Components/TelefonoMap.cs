using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class TelefonoMap : ComponentMapping<Telefono>
    {
         public TelefonoMap ()
         {
             Property(x => x.Numero);
             Property(x => x.Prefijo);
         }
    }
}