using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Roles
{
    public class ProfesionalMap : SubclassMapping<Profesional>
    {
        public ProfesionalMap()
        {
            DiscriminatorValue(Rol.PROFESIONAL);
            Property(x => x.Matricula);

            Set(x => x.Agendas, map =>
                                {
                                    map.Access(Accessor.NoSetter);
                                    map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                                }, rel => rel.OneToMany());
        }
    }
}