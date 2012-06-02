using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Areas.Admin.Models.Clinicas
{
	public class ClinicaEditViewModel
	{
		[UIHint("DropDownList")]        
        [DisplayName("Tipo Documento")]
        [DropDownProperty("DocumentoTipoDocumentoValue")]
        public IEnumerable<SelectListItem> TiposDocumentosHabilitados { get; set; }

		public SelectList ProvinciasHabilitadas { get; set; }
		public SelectList LocalidadesHabilitadas { get; set; }

		[Required]
		[DisplayName("Razón Social")]
		[DefaultStringLength]
		public virtual string RazonSocial { get; set; }

		[Required]
		[DisplayName("Tipo Documento")]
		public virtual int DocumentoTipoDocumentoValue { get; set; }


		[Required]
		[DisplayName("Número Documento")]
		public virtual long DocumentoNumero { get; set; }

		[Required]
		[DisplayName("Domicilio")]
		[DefaultStringLength]
		public virtual string DomicilioDireccion { get; set; }

		[Required]
		public virtual long DomicilioLocalidadId { get; set; }	

		[Required]
		[DisplayName("Domicilio Latitud")]
		public virtual double DomicilioLatitud { get; set; }

		[Required]
		[DisplayName("Domicilio Longitud")]
		public virtual double DomicilioLongitud { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Email")]
		[Email]
		[DefaultStringLength]
		public virtual string Email { get; set; }

		[Required]
		[DisplayName("Duracion Por Defecto Turno")]
		[DataType(DataType.Duration)]
		public virtual TimeSpan DuracionTurnoPorDefecto { get; set; }

		[Required]
		[DisplayName("Horario Matutino Desde")]
		public virtual DateTime HorarioMatutinoDesde { get; set; }

		[Required]
		[DisplayName("Horario Matutino Hasta")]
		public virtual DateTime HorarioMatutinoHasta { get; set; }

		[DisplayName("Horario Vespertino Desde")]
		public virtual DateTime? HorarioVespertinoDesde { get; set; }
		[DisplayName("Horario Vespertino Hasta")]
		public virtual DateTime? HorarioVespertinoHasta { get; set; }

		[Required]
		[DisplayName("Inasistencias Consecutivas")]
		[Range(1, int.MaxValue)]
		public virtual int NumeroInasistenciasConsecutivasGeneranBloqueo { get; set; }

		[Required]
		[DisplayName("Google Maps Key")]
		[DefaultStringLength]
		public virtual string GoogleMapsKey { get; set; }
	}
}