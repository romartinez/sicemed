using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class TurnoMap : EntityMapping<Turno>
    {
        public TurnoMap()
        {
            Property(x => x.FechaAtencion);
            Property(x => x.FechaGeneracion, map => map.NotNullable(true));
            Property(x => x.FechaIngreso);
            Property(x => x.FechaTurno, map => map.NotNullable(true));
            Property(x => x.IpPaciente, map => map.NotNullable(true));
            Property(x => x.Nota);

            ManyToOne(x => x.Paciente, map =>
            {
                map.NotNullable(true);
                map.ForeignKey("FK_Turnos_Usuario_Paciente");
            });
            ManyToOne(x => x.Secretaria, map =>
            {
                map.NotNullable(true);
                map.ForeignKey("FK_Turnos_Usuario_Secretaria");
            });
            
            ManyToOne(x => x.DiaAtencionEspecialidadProfesional, map => map.NotNullable(true));
        }
    }
}