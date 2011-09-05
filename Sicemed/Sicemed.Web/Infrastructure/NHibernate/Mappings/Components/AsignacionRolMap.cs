using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class AsignacionRolMap : ComponentMapping<AsignacionRol>
    {
        public AsignacionRolMap()
        {
            Property(x => x.FechaAsignacion, map =>
            {
                map.Access(Accessor.NoSetter);
                map.NotNullable(true);
            });
            Property(x => x.Rol, map =>
            {
                map.Type<EnumerationType<Rol>>();
                map.Access(Accessor.NoSetter);
                map.NotNullable(true);
            });
        }
    }
}