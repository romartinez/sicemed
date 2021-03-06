﻿using System;
using System.Collections.Generic;
using Sicemed.Web.Models.Enumerations;

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
            public DateTime? FechaCancelacion { get; set; }
            public DateTime? FechaAtencion { get; set; }
            public InfoViewModel Consultorio { get; set; }
            public InfoViewModel Profesional { get; set; }
            public InfoViewModel Especialidad { get; set; }
            public InfoViewModel CanceladoPor { get; set; }
            public string MotivoCancelacion { get; set; }
            public string Nota { get; set; }
            public EstadoTurno Estado { get; set; }
            public DateTime FechaEstado { get; set; }
            public bool PuedoCancelar { get; set; }
        }
    }
}