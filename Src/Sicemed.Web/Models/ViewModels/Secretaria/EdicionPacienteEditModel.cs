using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models.ViewModels.Secretaria
{
    public class EdicionPacienteEditModel
    {
        [Requerido]
        [HiddenInput]
        public long? Id { get; set; }

        [Requerido]
        [Display(Name = "Nombre", Prompt = "AAAA")]
        [LargoCadenaPorDefecto]
        public string Nombre { get; set; }

        [Display(Name = "Segundo Nombre")]
        [LargoCadenaPorDefecto]
        public string SegundoNombre { get; set; }

        [Requerido]
        [Display(Name = "Apellido")]
        [LargoCadenaPorDefecto]
        public string Apellido { get; set; }

        [UIHint("DropDownList")]
        [DisplayName("Tipo Documento")]
        [DropDownProperty("TipoDocumentoId")]
        public IEnumerable<SelectListItem> TiposDocumentosHabilitados { get; set; }

        [DisplayName("Tipo Documento")]
        [ScaffoldColumn(false)]
        public virtual long? TipoDocumentoId { get; set; }

        [DisplayName("Número Documento")]
        public virtual long? DocumentoNumero { get; set; }

        [Display(Name = "Fecha Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        [Requerido]
        [Correo]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [LargoCadenaPorDefecto]
        public string Email { get; set; }

        [DisplayName("Telefono")]
        public Telefono Telefono { get; set; }

        [DisplayName("Domicilio")]
        [LargoCadenaPorDefecto]
        public virtual string DomicilioDireccion { get; set; }

        [UIHint("DropDownList")]
        [DisplayName("Provincia")]
        [DropDownProperty("DomicilioLocalidadProvinciaId")]
        public IEnumerable<SelectListItem> ProvinciasHabilitadas { get; set; }

        [DisplayName("Localidad")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownProperty("DomicilioLocalidadId", "DomicilioLocalidadProvinciaId", "GetLocalidades", "Domain", "Admin", "provinciaId", "<< Seleccione una Provincia >>")]
        public IEnumerable<SelectListItem> LocalidadesHabilitadas { get; set; }

        [DisplayName("Localidad")]
        [ScaffoldColumn(false)]
        public virtual long? DomicilioLocalidadId { get; set; }

        [DisplayName("Provincia")]
        [ScaffoldColumn(false)]
        public virtual long? DomicilioLocalidadProvinciaId { get; set; }

        [UIHint("DropDownList")]
        [DisplayName("Obra Social")]
        [DropDownProperty("ObraSocialId")]
        public IEnumerable<SelectListItem> ObrasSocialesHabilitadas { get; set; }

        [DisplayName("Plan")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownProperty("PlanId", "ObraSocialId", "GetPlanesObraSocial", "Domain", "Admin", "obraSocialId", "<< Seleccione una Obra Social >>")]
        public IEnumerable<SelectListItem> PlanesObraSocialHabilitados { get; set; }

        [ScaffoldColumn(false)]
        public virtual long? PlanId { get; set; }

        [ScaffoldColumn(false)]
        public virtual long? ObraSocialId { get; set; }

        [DisplayName("Número Afiliado")]
        [LargoCadenaPorDefecto]
        public virtual string NumeroAfiliado { get; set; }
         
    }
}