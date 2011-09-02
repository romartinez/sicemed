using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class ParametroMap : ClassMapping<Parametro>
    {
         public ParametroMap()
         {
             Id(x => x.Key);
             Property("_value", map => map.Access(Accessor.Field));
         }
    }
}