using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Roles
{
    public class PacienteMap : SubclassMapping<Paciente>
    {
        public PacienteMap()
        {
            DiscriminatorValue(Rol.PACIENTE);
            Property(x => x.InasistenciasContinuas);
            Property(x => x.NumeroAfiliado);

            ManyToOne(x => x.Plan, map =>
                                       {
                                           map.ForeignKey("FK_PersonaRol_Paciente_Plan");
                                           map.Column("PlanId");
                                       });

            Set(x => x.Turnos, map =>
                               {
                                   map.Inverse(true);
                                   map.Access(Accessor.NoSetter);
                                   map.Key(k => k.Column("PacienteId"));
                               }, rel => rel.OneToMany());
        }
    }
}