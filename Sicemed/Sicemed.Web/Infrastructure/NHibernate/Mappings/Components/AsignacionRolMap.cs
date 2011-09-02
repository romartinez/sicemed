using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class AsignacionRolMap : ComponentMapping<AsignacionRol> 
    {
         public AsignacionRolMap()
         {
             Property(x => x.FechaAsignacion, map=>map.Access(Accessor.Field));
             Property(x => x.Rol, map => { 
                map.Type<EnumerationType<Rol>>();
                map.Access(Accessor.Field);
             });
         }
    }
}