using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Areas.Admin.Models.Clinicas
{
	public class ClinicaEditViewModel
	{

        [Required]
        [DisplayName("Razón Social")]
        [DefaultStringLength]
        public virtual string RazonSocial { get; set; }
        
        [UIHint("DropDownList")]        
        [DisplayName("Tipo Documento")]
        [DropDownProperty("DocumentoTipoDocumentoValue")]
        public IEnumerable<SelectListItem> TiposDocumentosHabilitados { get; set; }

        [Required]
        [DisplayName("Tipo Documento")]
        [ScaffoldColumn(false)]
        public virtual int DocumentoTipoDocumentoValue { get; set; }

        [Required]
        [DisplayName("Número Documento")]
        public virtual long DocumentoNumero { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        [Email]
        [DefaultStringLength]
        public virtual string Email { get; set; }

        [Required]
        [DisplayName("Telefonos")]
        [UIHint("Telefonos")]
        public IEnumerable<Telefono> Telefonos { get; set; }

        [Required]
        [DisplayName("Domicilio")]
        [DefaultStringLength]
        public virtual string DomicilioDireccion { get; set; }
        
        [UIHint("DropDownList")]
        [DisplayName("Provincia")]
        [DropDownProperty("DomicilioLocalidadProvinciaId")]
        public IEnumerable<SelectListItem> ProvinciasHabilitadas { get; set; }
		
        [DisplayName("Localidad")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownPropertyAttribute("DomicilioLocalidadId", "DomicilioLocalidadProvinciaId", "GetLocalidades", "Domain", "Admin", "provinciaId", "<< Seleccione una Provincia >>")]
        public IEnumerable<SelectListItem> LocalidadesHabilitadas { get; set; }

		[Required]
        [DisplayName("Localidad")]
        [ScaffoldColumn(false)]
		public virtual long? DomicilioLocalidadId { get; set; }
        
        [Required]
        [DisplayName("Provincia")]
        [ScaffoldColumn(false)]
        public virtual long? DomicilioLocalidadProvinciaId { get; set; }	

		[Required]
		[DisplayName("Domicilio Latitud")]
		public virtual double DomicilioLatitud { get; set; }

		[Required]
		[DisplayName("Domicilio Longitud")]
		public virtual double DomicilioLongitud { get; set; }

		[Required]
		[DisplayName("Horario Matutino Desde")]
        [DataType(DataType.Time)]
		public virtual DateTime HorarioMatutinoDesde { get; set; }
		[Required]
		[DisplayName("Horario Matutino Hasta")]
        [DataType(DataType.Time)]
        public virtual DateTime HorarioMatutinoHasta { get; set; }

		[DisplayName("Horario Vespertino Desde")]
        [DataType(DataType.Time)]
        public virtual DateTime? HorarioVespertinoDesde { get; set; }
		[DisplayName("Horario Vespertino Hasta")]
        [DataType(DataType.Time)]
        public virtual DateTime? HorarioVespertinoHasta { get; set; }

        [Required]
        [DisplayName("Duracion Por Defecto Turno")]
        [DataType(DataType.Duration)]
        public virtual TimeSpan DuracionTurnoPorDefecto { get; set; }

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