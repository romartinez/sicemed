﻿using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.Historial
{
    public class TurnosPorPacienteViewModel
    {
        public PacienteSearchViewModel SeleccionPaciente { get; set; }

        public SearchFiltersViewModel Filters { get; set; }

        public IEnumerable<HistorialItem> Turnos { get; set; }

        public TurnosPorPacienteViewModel()
        {
            Turnos = new List<HistorialItem>();
            Filters = new SearchFiltersViewModel();
            SeleccionPaciente = new PacienteSearchViewModel();
        }        

        public class HistorialItem
        {
            public DateTime FechaTurno { get; set; }
            public string Profesional { get; set; }
            public string Especialidad { get; set; }
            public string Consultorio { get; set; }
            public string Nota { get; set; }
            public bool SePresento { get; protected set; }
            public bool SeAtendio { get; protected set; }
            public bool EsObtenidoWeb { get; protected set; }
            public bool EsObtenidoPersonalmente { get; protected set; }
            public bool EsObtenidoTelefonicamente { get; protected set; }
        }
    }
}