using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Models.Enumerations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Models.Components;

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
            public decimal RetencionFija { get; set; }

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
            public EstadoTurno Estado { get; set; }
            public string MotivoCancelacion { get; set; }
            public DateTime FechaEstado { get; set; }
            public bool PuedoCancelar { get; set; }
            public bool PuedoPresentar { get; set; }
            public bool PuedoAtender { get; set; }
            public string NumeroAfiliado { get; set; }
            public decimal Coseguro { get; set; }
            public Plan Plan { get; set; }
            public ProfesionalViewModel Profesional { get; set; }
        }



    }
}