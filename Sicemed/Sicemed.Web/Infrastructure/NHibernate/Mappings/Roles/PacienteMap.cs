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
        }
    }
}