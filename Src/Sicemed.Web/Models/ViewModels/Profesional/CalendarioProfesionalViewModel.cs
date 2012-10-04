using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Models.Enumerations;

namespace Sicemed.Web.Models.ViewModels.Profesional
{
    public class CalendarioProfesionalViewModel
    {
        public long Id { get; set; }
        public string PersonaNombreCompleto { get; set; }

        public List<TurnoViewModel> Turnos { get; set; }

        public CalendarioProfesionalViewModel()
        {
            Turnos = new List<TurnoViewModel>();
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
            public EstadoTurno Estado { get; set; }
            public DateTime FechaEstado { get; set; }
        }
    }
}