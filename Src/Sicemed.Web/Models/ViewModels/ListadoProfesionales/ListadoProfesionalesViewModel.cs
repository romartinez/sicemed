using System;
using System.Collections.Generic;
using AutoMapper;

namespace Sicemed.Web.Models.ViewModels.ListadoProfesionales
{
//    [Serializable]
    public class ListadoProfesionalesViewModel
    {
/*        public DateTime FechaTurnoInicial { get; set; }
        public TimeSpan DuracionTurno { get; set; }
        public InfoViewModel Consultorio { get; set; }
        public InfoViewModel Paciente { get; set; }
        public List<InfoViewModel> EspecialidadesAtendidas { get; set; }

        public DateTime FechaTurnoFinal
        {
            get { return FechaTurnoInicial.Add(DuracionTurno); }
        }

        public bool SeSolapaConTurno(Turno turnoOtorgado)
        {
            //Los sobreturnos no computan
            if (turnoOtorgado.EsSobreTurno) return false;

            //El inicio del turno libre se encuentra dentro del período del turno otorgado.
            if (FechaTurnoInicial >= turnoOtorgado.FechaTurno && FechaTurnoInicial < turnoOtorgado.FechaTurnoFinal) return true;
            //El final del turno libre se encuentra dentro del período del turno otorgado.
            if (FechaTurnoFinal > turnoOtorgado.FechaTurno && FechaTurnoFinal <= turnoOtorgado.FechaTurnoFinal) return true;
            //El inicio del turno otorgado se encuentra dentro del período del turno libre
            if (turnoOtorgado.FechaTurno >= FechaTurnoInicial && turnoOtorgado.FechaTurno < FechaTurnoFinal) return true;
            //El inicio del turno otorgado se encuentra dentro del período del turno libre
            if (turnoOtorgado.FechaTurnoFinal > FechaTurnoInicial && turnoOtorgado.FechaTurnoFinal <= FechaTurnoFinal) return true;
            
            //No se solapan
            return false;
        }

        public static TurnoViewModel Create(DateTime diaConHora, Agenda agendaDia)
        {
            var vm = new TurnoViewModel();
            vm.FechaTurnoInicial = diaConHora;
            vm.DuracionTurno = agendaDia.DuracionTurno;
            vm.Consultorio = Mapper.Map<InfoViewModel>(agendaDia.Consultorio);
            vm.EspecialidadesAtendidas = Mapper.Map<List<InfoViewModel>>(agendaDia.EspecialidadesAtendidas);
            return vm;
        }
   */ }
}