using System;

namespace Sicemed.Web.Models.ViewModels.ObtenerTurno
{
    public class TurnoViewModel
    {
        public DateTime FechaTurnoInicial { get; set; }
        public DateTime FechaTurnoFinal { get; set; }
        public InfoViewModel Consultorio { get; set; }
        public InfoViewModel Paciente { get; set; }
        public InfoViewModel Agenda { get; set; }
        public InfoViewModel Especialidad { get; set; }

        public static TurnoViewModel Create(Turno t)
        {
            var vm = new TurnoViewModel
                         {
                             FechaTurnoInicial = t.FechaTurno,
                             FechaTurnoFinal = t.FechaTurno.AddMinutes(t.Agenda.DuracionTurno.TotalMinutes),
                             Consultorio = new InfoViewModel
                                               {
                                                   Id = t.Consultorio.Id,
                                                   Descripcion = t.Consultorio.Nombre
                                               },
                             Agenda = new InfoViewModel
                                          {
                                              Id = t.Agenda.Id
                                          },
                             Paciente = new InfoViewModel
                                            {
                                                Id = t.Paciente.Id,
                                                Descripcion = t.Paciente.Persona.Nombre
                                            }
                         };
            return vm;
        }

        public static TurnoViewModel Create(DateTime diaConHora, Agenda agendaDia)
        {
            var vm = new TurnoViewModel
                         {
                             FechaTurnoInicial = diaConHora,
                             FechaTurnoFinal = diaConHora.AddMinutes(agendaDia.DuracionTurno.TotalMinutes),
                             Agenda = new InfoViewModel { Id = agendaDia.Id },
                             Consultorio = new InfoViewModel
                                               {
                                                   Id = agendaDia.Consultorio.Id,
                                                   Descripcion = agendaDia.Consultorio.Nombre
                                               }
                         };
            return vm;
        }
    }
}