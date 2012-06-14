using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.HistorialAtenciones
{
    public class HistorialAtencionesViewModel
    {
        public string Paciente { get; set; }
        public IEnumerable<HistorialItem> Turnos { get; set; }

        public HistorialAtencionesViewModel()
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