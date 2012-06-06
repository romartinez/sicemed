using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Roles
{
    public class RolMap : EntityMapping<Rol>
    {
        public RolMap()
        {
            Discriminator(x => x.Column("Rol"));
            Property(x => x.FechaAsignacion);

            ManyToOne(x => x.Persona, map => { 
                map.ForeignKey("FK_Roles_Persona");
            });
        }
    }
}