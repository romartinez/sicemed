using System;
using System.Collections.Generic;
using AutoMapper;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Roles;


namespace Sicemed.Web.Models.ViewModels.ListadoObraSociales
{
    //    [Serializable]
    public class ListadoObraSocialesViewModel
    {
        public List<ObraSocialesViewModel> Profesionales { get; set; }

        public ListadoObraSocialesViewModel()
        {
            Profesionales = new List<ObraSocialesViewModel>();
        }

        public class ObraSocialesViewModel
        {
            public long Id { get; set; }
            public string DisplayName { get; set; }
            public virtual DateTime FechaAsignacion { get; set; }
            public string Matricula { get; set; }
            public decimal RetencionFija { get; set; }
            public IEnumerable<Models.Especialidad> Especialidades { get; set; }
        }



        /*       
         * public long Id { get; set; }
                public long Documento { get; set; }
                public string NombreCompleto { get; set; }
                public string TipoDocumento { get; set; }
                public List<InfoViewModel> Especialidades { get; set; }

                public ProfesionalConEspecialidadesViewModel()
                {
                    Especialidades = new List<InfoViewModel>();
                }
         * 
         * 
         * public DateTime FechaTurnoInicial { get; set; }
                public TimeSpan DuracionTurno { get; set; }
                public InfoViewModel Consultorio { get; set; }
                public InfoViewModel Paciente { get; set; }
                public List<InfoViewModel> EspecialidadesAtendidas { get; set; }

                public DateTime FechaTurnoFinal
                {
                    get { return FechaTurnoInicial.Add(DuracionTurno); }
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
           */
    }
}