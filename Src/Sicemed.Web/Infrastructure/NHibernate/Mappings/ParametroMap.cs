using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class ParametroMap : ClassMapping<Parametro>
    {
        public ParametroMap()
        {
            Id(x => x.Key, map => map.Type(new EnumStringType<Parametro.Keys>()));
            Property("_value", map =>
                               {
                                   map.Access(Accessor.NoSetter);
                                   map.NotNullable(true);
                               });
        }
    }
}