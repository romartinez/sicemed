using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class TurnoMap : EntityMapping<Turno>
    {
         public TurnoMap()
         {
             Property(x => x.FechaAtencion);
             Property(x => x.FechaGeneracion);
             Property(x => x.FechaIngreso);
             Property(x => x.FechaTurno);
             Property(x => x.IpPaciente);
             Property(x => x.Nota);

             ManyToOne(x => x.Paciente);
             ManyToOne(x => x.Secretaria);
             ManyToOne(x => x.DiaAtencionEspecialidadProfesional);
         }
    }
}