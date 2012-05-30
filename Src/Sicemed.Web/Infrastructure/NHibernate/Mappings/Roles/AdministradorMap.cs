using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Roles
{
    public class AdministradorMap : SubclassMapping<Administrador>
    {
        public AdministradorMap()
        {
            DiscriminatorValue(Rol.ADMINISTRADOR);
        }
    }
}