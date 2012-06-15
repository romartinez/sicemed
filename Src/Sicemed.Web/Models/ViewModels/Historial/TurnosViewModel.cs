using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.Historial
{
    public class TurnosViewModel
    {
        public SearchFiltersViewModel Filters { get; set; }

        public InfoViewModel Paciente { get; set; }

        public IEnumerable<HistorialItem> Turnos { get; set; }

        public TurnosViewModel()
        {
            Turnos = new List<HistorialItem>();
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