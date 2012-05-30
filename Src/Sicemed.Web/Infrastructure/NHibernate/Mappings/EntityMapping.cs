using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class EntityMapping<T> : ClassMapping<T> where T : Entity
    {
        public EntityMapping()
        {
            Id(x => x.Id, map => map.Generator(Generators.HighLow));
        }
    }
}