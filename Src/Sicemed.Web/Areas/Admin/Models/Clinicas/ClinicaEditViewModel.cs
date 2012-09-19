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

        [Requerido]
        [DisplayName("Razón Social")]
        [LargoCadenaPorDefecto]
        public virtual string RazonSocial { get; set; }
        
        [UIHint("DropDownList")]        
        [DisplayName("Tipo Documento")]
        [DropDownProperty("DocumentoTipoDocumentoValue")]
        public IEnumerable<SelectListItem> TiposDocumentosHabilitados { get; set; }

        [Requerido]
        [DisplayName("Tipo Documento")]
        [ScaffoldColumn(false)]
        public virtual int DocumentoTipoDocumentoValue { get; set; }

        [Requerido]
        [DisplayName("Número Documento")]
        public virtual long DocumentoNumero { get; set; }

        [Requerido]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        [Correo]
        [LargoCadenaPorDefecto]
        public virtual string Email { get; set; }

        [Requerido]
        [DisplayName("Telefonos")]
        [UIHint("Telefonos")]
        public IEnumerable<Telefono> Telefonos { get; set; }

        [Requerido]
        [DisplayName("Domicilio")]
        [LargoCadenaPorDefecto]
        public virtual string DomicilioDireccion { get; set; }
        
        [UIHint("DropDownList")]
        [DisplayName("Provincia")]
        [DropDownProperty("DomicilioLocalidadProvinciaId")]
        public IEnumerable<SelectListItem> ProvinciasHabilitadas { get; set; }
		
        [DisplayName("Localidad")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownPropertyAttribute("DomicilioLocalidadId", "DomicilioLocalidadProvinciaId", "GetLocalidades", "Domain", "Admin", "provinciaId", "<< Seleccione una Provincia >>")]
        public IEnumerable<SelectListItem> LocalidadesHabilitadas { get; set; }

		[Requerido]
        [DisplayName("Localidad")]
        [ScaffoldColumn(false)]
		public virtual long? DomicilioLocalidadId { get; set; }
        
        [Requerido]
        [DisplayName("Provincia")]
        [ScaffoldColumn(false)]
        public virtual long? DomicilioLocalidadProvinciaId { get; set; }	

		[Requerido]
		[DisplayName("Domicilio Latitud")]
		public virtual double DomicilioLatitud { get; set; }

		[Requerido]
		[DisplayName("Domicilio Longitud")]
		public virtual double DomicilioLongitud { get; set; }

        [UIHint("MultipleList")]
        [DisplayName("Dias Habilitados")]
        [DropDownProperty("DiasHabilitados")]
        public IEnumerable<SelectListItem> DiasHabilitadosPosibles { get; set; }

        [Requerido]
        [ScaffoldColumn(false)]
        public DayOfWeek[] DiasHabilitados { get; set; }

		[Requerido]
		[DisplayName("Horario Matutino Desde")]
        [DataType(DataType.Time)]
		public virtual DateTime HorarioMatutinoDesde { get; set; }
		[Requerido]
		[DisplayName("Horario Matutino Hasta")]
        [DataType(DataType.Time)]
        public virtual DateTime HorarioMatutinoHasta { get; set; }

		[DisplayName("Horario Vespertino Desde")]
        [DataType(DataType.Time)]
        public virtual DateTime? HorarioVespertinoDesde { get; set; }
		[DisplayName("Horario Vespertino Hasta")]
        [DataType(DataType.Time)]
        public virtual DateTime? HorarioVespertinoHasta { get; set; }

        [Requerido]
        [DisplayName("Duracion Por Defecto Turno")]
        [DataType(DataType.Duration)]
        public virtual TimeSpan DuracionTurnoPorDefecto { get; set; }

        [Requerido]
        [DisplayName("Inasistencias Consecutivas")]
        [Range(1, int.MaxValue)]
        public virtual int NumeroInasistenciasConsecutivasGeneranBloqueo { get; set; }
        
        [Requerido]
		[DisplayName("Google Maps Key")]
		[LargoCadenaPorDefecto]
		public virtual string GoogleMapsKey { get; set; }
	}
}