﻿using Sicemed.Web.Models;

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
            Property(x => x.IpPaciente);
            Property(x => x.EsTelefonico);
            Property(x => x.Nota);

            ManyToOne(x => x.Paciente, map =>
                                       {
                                           map.NotNullable(true);
                                           map.ForeignKey("FK_Turnos_Paciente");
                                       });

            ManyToOne(x => x.SecretariaReservadoraTurno, map =>
                                         {
                                             map.ForeignKey("FK_Turnos_SecretariaReservadoraTurno");
                                         });

            ManyToOne(x => x.SecretariaRecepcionista, map =>
                                         {
                                             map.ForeignKey("FK_Turnos_SecretariaRecepcionista");
                                         });

            ManyToOne(x => x.Profesional, map =>
                                          {
                                              map.NotNullable(true);
                                              map.ForeignKey("FK_Turnos_Profesional");
                                          });

            ManyToOne(x => x.Especialidad, map =>
                                           {
                                               map.NotNullable(true);
                                               map.ForeignKey("FK_Turnos_Especialidad");
                                           });

            ManyToOne(x => x.Consultorio, map => { map.ForeignKey("FK_Turnos_Consultorio"); });
            
            ManyToOne(x => x.Agenda, map => { map.ForeignKey("FK_Turnos_Agenda"); });
        }
    }
}