using System;

namespace Sicemed.Web.Models.Reports
{
    public class FichaPacienteReportModel
    {
        public string NombrePaciente { get; set; }
        public string DocumentoTipo { get; set; }
        public string DocumentoNumero { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string FechaNacimiento { get; set; }
        public string Edad { get; set; }
        public string Peso { get; set; }
        public string Altura { get; set; }
        public string Domicilio { get; set; }
        public string ObraSocial { get; set; }
        public string Plan { get; set; }
        public string NumeroAfiliado { get; set; }

        public DateTime FechaTurno { get; set; }
        public string Profesional { get; set; }
        public string Consultorio { get; set; }
        public string Especialidad { get; set; }        
        public string Nota { get; set; }
    }
}