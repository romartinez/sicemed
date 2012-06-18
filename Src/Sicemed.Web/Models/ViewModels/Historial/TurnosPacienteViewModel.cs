using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.Historial
{
    public class TurnosPacienteViewModel
    {
        public SearchFiltersViewModel Filters { get; set; }

        public IEnumerable<HistorialItem> Turnos { get; set; }

        public TurnosPacienteViewModel()
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
            public bool SePresento { get; protected set; }
            public bool SeAtendio { get; protected set; }
            public bool EsObtenidoWeb { get; protected set; }
            public bool EsObtenidoPersonalmente { get; protected set; }
            public bool EsObtenidoTelefonicamente { get; protected set; }
        }
    }
}