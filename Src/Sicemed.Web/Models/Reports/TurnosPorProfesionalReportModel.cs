using System;

namespace Sicemed.Web.Models.Reports
{
    public class TurnosPorProfesionalReportModel
    {
        public string Nombre { get; set; }
        public DateTime FechaTurno { get; set; }
        public string Paciente { get; set; }
    }
}