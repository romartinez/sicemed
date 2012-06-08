using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class ProvinciaMap : EntityMapping<Provincia>
    {
        public ProvinciaMap()
        {
            Property(x => x.Nombre, map => map.NotNullable(true));

            Set(x => x.Localidades, map =>
                                    {
                                        map.Inverse(true);
                                        map.Access(Accessor.NoSetter);
                                        map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                                    }, rel => rel.OneToMany());
        }
    }
}