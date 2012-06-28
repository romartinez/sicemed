using System;
using System.Collections.Generic;
using System.Linq;

namespace Sicemed.Web.Models.ViewModels.Secretaria
{
    public class TurnosDelDiaViewModel
    {
        public DateTime FechaTurnos { get; set; }

        public List<ProfesionalViewModel> ProfesionalesConTurnos { get; set; }

        public TurnosDelDiaViewModel()
        {
            ProfesionalesConTurnos = new List<ProfesionalViewModel>();
        }

        public class ProfesionalViewModel
        {
            public long Id { get; set; }
            public string PersonaNombreCompleto { get; set; }

            public long PacientesPendientes
            {
                get
                {
                    if (Turnos == null) return 0;
                    return Turnos.Count(t => t.Estado == Turno.EstadoTurno.Presentado);
                }
            }

            public long TurnosPendientes
            {
                get
                {
                    if (Turnos == null) return 0;
                    return Turnos.Count(t => t.Estado == Turno.EstadoTurno.Otorgado);
                }
            }

            public List<TurnoViewModel> Turnos { get; set; }

            public ProfesionalViewModel()
            {
                Turnos = new List<TurnoViewModel>();
            }
        }

        public class TurnoViewModel
        {
            public long Id { get; set; }
            public DateTime FechaTurno { get; set; }
            public DateTime? FechaIngreso { get; set; }
            public DateTime? FechaAtencion { get; set; }
            public DateTime? FechaCancelacion { get; set; }
            public InfoViewModel Consultorio { get; set; }
            public InfoViewModel Paciente { get; set; }
            public InfoViewModel Especialidad { get; set; }
            public InfoViewModel CanceladoPor { get; set; }
            public Turno.EstadoTurno Estado { get; set; }
            public string MotivoCancelacion { get; set; }
            public DateTime FechaEstado { get; set; }
            public bool PuedoCancelar { get; set; }
            public bool PuedoPresentar { get; set; }
            public bool PuedoAtender { get; set; }
        }
    }
}