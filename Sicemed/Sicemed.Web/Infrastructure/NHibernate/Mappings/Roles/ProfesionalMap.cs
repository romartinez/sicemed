using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Roles
{
    public class ProfesionalMap : SubclassMapping<Profesional>
    {
        public ProfesionalMap()
        {
            DiscriminatorValue("Profesional");
            Property(x => x.Matricula);
        }
    }
}