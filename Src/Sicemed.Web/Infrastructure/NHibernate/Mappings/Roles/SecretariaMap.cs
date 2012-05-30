using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Roles
{
    public class SecretariaMap : SubclassMapping<Secretaria>
    {
        public SecretariaMap()
        {
            DiscriminatorValue(Rol.SECRETARIA);
            Property(x => x.FechaIngreso);
        }
    }
}