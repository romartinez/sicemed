using System;
using System.Collections.Generic;
using System.Linq;

namespace Sicemed.Web.Models.ViewModels.ObtenerTurno
{
    [Serializable]
    public class TurnoViewModel
    {
        public DateTime FechaTurnoInicial { get; set; }
        public DateTime FechaTurnoFinal { get; set; }
        public InfoViewModel Consultorio { get; set; }
        public InfoViewModel Paciente { get; set; }
        public AgendaConEspecialidadesViewModel Agenda { get; set; }
        public InfoViewModel Especialidad { get; set; }

        public static TurnoViewModel Create(Turno t)
        {
            var vm = new TurnoViewModel();
            vm.FechaTurnoInicial = t.FechaTurno;
            vm.FechaTurnoFinal = t.FechaTurno.AddMinutes(t.Agenda.DuracionTurno.TotalMinutes);
            vm.Consultorio = new InfoViewModel
                                 {
                                     Id = t.Consultorio.Id,
                                     Descripcion = t.Consultorio.Nombre
                                 };
            vm.Agenda = new AgendaConEspecialidadesViewModel
                            {
                                Id = t.Agenda.Id,
                                EspecialidadesAtendidas = t.Agenda.EspecialidadesAtendidas
                                    .Select(x => new InfoViewModel() { Id = x.Id, Descripcion = x.Nombre }).ToList()
                            };
            vm.Paciente = new InfoViewModel
                              {
                                  Id = t.Paciente.Id,
                                  Descripcion = t.Paciente.Persona.Nombre
                              };
            return vm;
        }

        public static TurnoViewModel Create(DateTime diaConHora, Agenda agendaDia, Especialidad especialidad)
        {
            var vm = new TurnoViewModel();
            vm.FechaTurnoInicial = diaConHora;
            vm.FechaTurnoFinal = diaConHora.AddMinutes(agendaDia.DuracionTurno.TotalMinutes);
            vm.Agenda = new AgendaConEspecialidadesViewModel
                            {
                                Id = agendaDia.Id,
                                EspecialidadesAtendidas = agendaDia.EspecialidadesAtendidas
                                    .Select(x => new InfoViewModel() { Id = x.Id, Descripcion = x.Nombre }).ToList()
                            };
            vm.Especialidad = new InfoViewModel
                                  {
                                      Id = especialidad.Id,
                                      Descripcion = especialidad.Nombre
                                  };
            vm.Consultorio = new InfoViewModel
                                 {
                                     Id = agendaDia.Consultorio.Id,
                                     Descripcion = agendaDia.Consultorio.Nombre
                                 };
            return vm;
        }

        public static IEnumerable<TurnoViewModel> Create(DateTime diaConHora, Agenda agendaDia)
        {
            return agendaDia.EspecialidadesAtendidas.Select(especialidad => Create(diaConHora, agendaDia, especialidad));
        }
    }
}