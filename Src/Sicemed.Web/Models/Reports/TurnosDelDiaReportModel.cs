using System;

namespace Sicemed.Web.Models.Reports
{
    public class TurnosDelDiaReportModel
    {
        public DateTime FechaListado { get; set; }
        public string NombreProfesional { get; set; }
        public DateTime FechaInicioTurno { get; set; }
        public DateTime FechaFinTurno { get; set; }
        public string NombrePaciente { get; set; }
    }
}