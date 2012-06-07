using System;
using System.Collections.Generic;
using System.Linq;

namespace Sicemed.Web.Models.ViewModels.Secretaria
{
    public class TurnosDelDiaViewModel
    {
        public List<ProfesionalViewModel> ProfesionalesConTurnos { get; set; }
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
                return Turnos.Count(t => t.SePresento && !t.SeAtendio);
            }
        }

        public long TurnosPendientes
        {
            get
            {
                if (Turnos == null) return 0;
                return Turnos.Count(t => !t.SePresento);
            }
        }

        public List<TurnoViewModel> Turnos { get; set; }
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

        public bool SePresento
        {
            get { return FechaIngreso.HasValue; }
        }

        public bool SeAtendio
        {
            get { return FechaAtencion.HasValue; }
        }
    }
}