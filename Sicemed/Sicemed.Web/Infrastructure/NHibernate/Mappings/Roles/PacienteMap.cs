using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Roles
{
    public class PacienteMap : SubclassMapping<Paciente>
    {
        public PacienteMap()
        {
            DiscriminatorValue(new Paciente().DisplayName);            
            Property(x => x.InasistenciasContinuas);
            Property(x => x.NumeroAfiliado);
            Property(x => x.EstaHabilitadoTurnosWeb);            
        }
    }
}