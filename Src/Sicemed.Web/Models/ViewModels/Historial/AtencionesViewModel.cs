using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.Historial
{
    public class AtencionesViewModel
    {
        public SearchFiltersViewModel Filters { get; set; }

        public long PacienteId { get; set; }
        public string PacienteNombre { get; set; }
        
        public IEnumerable<HistorialItem> Turnos { get; set; }

        public AtencionesViewModel()
        {
            Turnos = new List<HistorialItem>();
            Filters = new SearchFiltersViewModel();
        }

        public class HistorialItem
        {
            public DateTime FechaTurno { get; set; }
            public string Profesional { get; set; }
            public string Especialidad { get; set; }
            public string Consultorio { get; set; }
            public string Nota { get; set; }
        }
    }
}