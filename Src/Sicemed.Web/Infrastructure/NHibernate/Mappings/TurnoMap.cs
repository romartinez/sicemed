using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class TurnoMap : EntityMapping<Turno>
    {
        public TurnoMap()
        {
            Table("Turnos");

            Property(x => x.FechaAtencion);
            Property(x => x.FechaGeneracion, map => map.NotNullable(true));
            Property(x => x.FechaIngreso);
            Property(x => x.FechaCancelacion);
            Property(x => x.FechaTurno, map => map.NotNullable(true));
            Property(x => x.IpPaciente);
            Property(x => x.EsTelefonico);
            Property(x => x.Nota);
            Property(x => x.MotivoCancelacion);
            Property(x => x.Estado);
            Property(x => x.FechaEstado);

            ManyToOne(x => x.Paciente, map =>
                                       {
                                           map.NotNullable(true);
                                           map.ForeignKey("FK_Turno_PersonaRol_Paciente");
                                       });

            ManyToOne(x => x.SecretariaReservadoraTurno, map => map.ForeignKey("FK_Turno_PersonaRol_Secretaria_ReservadoraTurno"));

            ManyToOne(x => x.SecretariaRecepcionista, map => map.ForeignKey("FK_Turno_PersonaRol_Secretaria_Recepcionista"));

            ManyToOne(x => x.CanceladoPor, map => map.ForeignKey("FK_Turno_Persona_CanceladoPor"));

            ManyToOne(x => x.Profesional, map =>
                                          {
                                              map.NotNullable(true);
                                              map.ForeignKey("FK_Turno_PersonaRol_Profesional");
                                          });

            ManyToOne(x => x.Especialidad, map =>
                                           {
                                               map.NotNullable(true);
                                               map.ForeignKey("FK_Turno_Especialidad");
                                           });

            ManyToOne(x => x.Consultorio, map => map.ForeignKey("FK_Turno_Consultorio"));            
        }
    }
}