using System;
using System.Collections.Generic;
using Sicemed.Web.Models.Enumerations;

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
            public InfoViewModel Profesional { get; set; }
            public InfoViewModel CanceladoPor { get; set; }
            public DateTime? FechaCancelacion { get; set; }
            public string MotivoCancelacion { get; set; }
            public InfoViewModel Especialidad { get; set; }
            public InfoViewModel Consultorio { get; set; }
            public string Nota { get; set; }
            public EstadoTurno Estado { get; protected set; }
            public DateTime FechaEstado { get; protected set; }
            public bool PuedoCancelar { get; set; }
            public bool PuedoPresentar { get; set; }
            public bool PuedoAtender { get; set; }
            public bool EsObtenidoWeb { get; protected set; }
            public bool EsObtenidoPersonalmente { get; protected set; }
            public bool EsObtenidoTelefonicamente { get; protected set; }
        }
    }
}