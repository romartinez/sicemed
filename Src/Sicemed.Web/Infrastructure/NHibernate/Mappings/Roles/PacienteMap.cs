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
            Property(x => x.EstaHabilitadoTurnosWeb);

            ManyToOne(x => x.Plan, map => map.ForeignKey("FK_Pacientes_Plan"));

            Set(x => x.Turnos, map =>
                               {
                                   map.Inverse(true);
                                   map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                                   map.Access(Accessor.NoSetter);
                               }, rel => rel.OneToMany());
        }
    }
}