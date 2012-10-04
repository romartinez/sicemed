﻿using System;
using System.Collections.Generic;
using Sicemed.Web.Models.Enumerations;

namespace Sicemed.Web.Models.ViewModels.Historial
{
    public class TurnosPacienteViewModel
    {
        public SearchFiltersViewModel Filters { get; set; }

        public IEnumerable<HistorialItem> Turnos { get; set; }

        public TurnosPacienteViewModel()
        {
            Turnos = new List<HistorialItem>();
            Filters = new SearchFiltersViewModel();
        }

        public class HistorialItem
        {
            public DateTime FechaTurno { get; set; }
            public DateTime? FechaCancelacion { get; set; }
            public string Profesional { get; set; }
            public string Especialidad { get; set; }
            public string Consultorio { get; set; }
            public string Nota { get; set; }
            public string MotivoCancelacion { get; set; }
            public InfoViewModel CanceladoPor { get; set; }
            public bool EsObtenidoWeb { get; protected set; }
            public bool EsObtenidoPersonalmente { get; protected set; }
            public bool EsObtenidoTelefonicamente { get; protected set; }
            public EstadoTurno Estado { get; protected set; }
            public DateTime FechaEstado { get; protected set; }
            public bool PuedoCancelar { get; set; }
            public bool PuedoPresentar { get; set; }
            public bool PuedoAtender { get; set; }
        }
    }
}