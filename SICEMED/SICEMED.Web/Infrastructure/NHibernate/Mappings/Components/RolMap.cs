using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Components.Roles;
using Sicemed.Web.Models.Enumerations.Documentos;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class RolMap : ComponentMapping<Rol>
    {
        public RolMap()
        {
            Property(x => x.DisplayName, map=>map.Access(Accessor.NoSetter));
            Property(x => x.FechaAsignacion);
        }
    }
}