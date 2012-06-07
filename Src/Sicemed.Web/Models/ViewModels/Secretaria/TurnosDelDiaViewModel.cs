using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.Secretaria
{
    public class TurnosDelDiaViewModel
    {
        public List<ProfesionalViewModel> ProfesionalesConTurnos { get; set; }

        //public TurnosDelDiaViewModel()
        //{
        //    ProfesionalesConTurnos = new List<ProfesionalViewModel>();
        //}
    }

    public class ProfesionalViewModel
    {
        public long Id { get; set; }
        public string PersonaNombreCompleto { get; set; }

        public List<TurnoViewModel> Turnos { get; set; }

        //public ProfesionalViewModel()
        //{
        //    Turnos = new List<TurnoViewModel>();
        //}
    }

    public class TurnoViewModel
    {
        public long Id { get; set; }
        public DateTime FechaTurno { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaAtencion { get; set; }
        public InfoViewModel Consultorio { get; set; }
        public InfoViewModel Paciente { get; set; }
        public InfoViewModel Especialidad { get; set; }
    }
}