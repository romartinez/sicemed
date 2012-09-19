using System;
using System.Collections.Generic;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models.ViewModels
{
	public class ClinicaViewModel
	{
        public string RazonSocial { get; set; }
        public int DocumentoTipoDocumentoValue { get; set; }
        public long DocumentoNumero { get; set; }
        public string Email { get; set; }
        public IEnumerable<Telefono> Telefonos { get; set; }
        public IEnumerable<DayOfWeek> DiasHabilitados { get; set; }

        public string DomicilioDireccion { get; set; }        
		public string DomicilioLocalidadNombre { get; set; }        
        public string DomicilioLocalidadProvinciaNombre { get; set; }

		public double DomicilioLatitud { get; set; }
		public double DomicilioLongitud { get; set; }

        public TimeSpan HorarioMatutinoDesde { get; set; }
        public TimeSpan HorarioMatutinoHasta { get; set; }

        public TimeSpan? HorarioVespertinoDesde { get; set; }
        public TimeSpan? HorarioVespertinoHasta { get; set; }

        public bool EsHorarioCorrido { get; set; }

        public TimeSpan DuracionTurnoPorDefecto { get; set; }
        public int NumeroInasistenciasConsecutivasGeneranBloqueo { get; set; }        
		public string GoogleMapsKey { get; set; }
	}
}