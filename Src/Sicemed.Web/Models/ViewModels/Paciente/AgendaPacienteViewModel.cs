using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.Paciente
{
    public class AgendaPacienteViewModel
    {
        public string PersonaNombreCompleto { get; set; }
        public DateTime FechaTurnos { get; set; }

        public List<TurnoViewModel> Turnos { get; set; }

        public AgendaPacienteViewModel()
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
            public InfoViewModel Profesional { get; set; }
            public InfoViewModel Especialidad { get; set; }
            public Turno.EstadoTurno Estado { get; set; }
        }
    }
}