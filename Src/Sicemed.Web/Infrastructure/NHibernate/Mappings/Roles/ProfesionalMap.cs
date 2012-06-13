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
                                    map.Inverse(true);
                                    map.Key(k =>
                                    {
                                        k.ForeignKey("FK_PersonaRoles_Profesional");
                                        k.Column("ProfesionalId");
                                    });
                                }, rel => rel.OneToMany());

            Set(x => x.Especialidades, map =>
            {
                map.Access(Accessor.NoSetter);
                map.Key(k =>
                {
                    k.ForeignKey("FK_ProfesionalEspecialidad_PersonaRol");
                    k.Column("ProfesionalId");
                });
                map.Table("ProfesionalEspecialidades");
            }, rel => rel.ManyToMany());

        }
    }
}