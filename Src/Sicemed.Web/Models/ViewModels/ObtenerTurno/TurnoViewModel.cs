using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Sicemed.Web.Models.ViewModels.ObtenerTurno
{
    [Serializable]
    public class TurnoViewModel
    {
        public DateTime FechaTurnoInicial { get; set; }
        public DateTime FechaTurnoFinal { get; set; }
        public InfoViewModel Consultorio { get; set; }
        public InfoViewModel Paciente { get; set; }        
        public List<InfoViewModel> EspecialidadesAtendidas { get; set; }

        public static IEnumerable<TurnoViewModel> Create(DateTime diaConHora, Agenda agendaDia)
        {
            return agendaDia.EspecialidadesAtendidas.Select(especialidad =>
            {
                var vm = new TurnoViewModel();
                vm.FechaTurnoInicial = diaConHora;
                vm.FechaTurnoFinal = diaConHora.AddMinutes(agendaDia.DuracionTurno.TotalMinutes);
                vm.Consultorio = Mapper.Map<InfoViewModel>(agendaDia.Consultorio);
                vm.EspecialidadesAtendidas = Mapper.Map<List<InfoViewModel>>(agendaDia.EspecialidadesAtendidas);
                return vm;
            });
        }
    }
}