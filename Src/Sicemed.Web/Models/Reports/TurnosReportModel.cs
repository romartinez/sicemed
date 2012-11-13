using System;

namespace Sicemed.Web.Models.Reports
{
    public class TurnosReportModel
    {
        public DateTime FechaTurnos { get; set; }
        public string Profesional { get; set; }
        public string Consultorio { get; set; }        
        public string Especialidad { get; set; }
        public string Paciente { get; set; }
        public DateTime FechaTurno { get; set; }
        public DateTime FechaTurnoFin { get; set; }
    }
}