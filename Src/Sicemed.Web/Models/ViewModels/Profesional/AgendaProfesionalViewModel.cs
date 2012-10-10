using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Models.Enumerations;

namespace Sicemed.Web.Models.ViewModels.Profesional
{
    public class AgendaProfesionalViewModel
    {
        public long Id { get; set; }
        public string PersonaNombreCompleto { get; set; }
        public DateTime FechaTurnos { get; set; }

        public long PacientesPendientes
        {
            get
            {
                if (Turnos == null) return 0;
                return Turnos.Count(t => t.Estado == EstadoTurno.Presentado);
            }
        }

        public long TurnosPendientes
        {
            get
            {
                if (Turnos == null) return 0;
                return Turnos.Count(t => t.Estado == EstadoTurno.Otorgado);
            }
        }

        public List<TurnoViewModel> Turnos { get; set; }

        public AgendaProfesionalViewModel()
        {
            Turnos = new List<TurnoViewModel>();
        }

        public class TurnoViewModel
        {
            public long Id { get; set; }
            public DateTime FechaTurno { get; set; }
            public TimeSpan DuracionTurno { get; set; }
            public DateTime? FechaIngreso { get; set; }
            public DateTime? FechaAtencion { get; set; }
            public InfoViewModel Consultorio { get; set; }
            public InfoViewModel Paciente { get; set; }
            public InfoViewModel Especialidad { get; set; }
            public EstadoTurno Estado { get; set; }
            public DateTime FechaEstado { get; set; }
            public bool PuedoCancelar { get; set; }

            public DateTime FechaTurnoFinal
            {
                get { return FechaTurno.Add(DuracionTurno); }
            }
        }
    }
}