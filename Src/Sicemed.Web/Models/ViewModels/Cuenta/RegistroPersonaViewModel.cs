using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using NHibernate.Type;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models.ViewModels.Cuenta
{
    public class RegistroPersonaViewModel
    {
        [Required]
        [Display(Name = "Nombre", Prompt = "AAAA")]
        [DefaultStringLength]
        public string Nombre { get; set; }

        [Display(Name = "Segundo Nombre")]
        [DefaultStringLength]
        public string SegundoNombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        [DefaultStringLength]
        public string Apellido { get; set; }

        [UIHint("DropDownList")]
        [DisplayName("Tipo Documento")]
        [DropDownProperty("TipoDocumentoId")]
        public IEnumerable<SelectListItem> TiposDocumentosHabilitados { get; set; }

        [Required]
        [DisplayName("Tipo Documento")]
        [ScaffoldColumn(false)]
        public virtual int TipoDocumentoId { get; set; }

        [Required]
        [DisplayName("Número Documento")]
        public virtual long DocumentoNumero { get; set; }

        [Display(Name = "Fecha Nacimiento")]        
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        [Required]
        [Email]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [DefaultStringLength]
        public string Email { get; set; }

        [Required]
        [MinLength(MembershipService.MIN_REQUIRED_PASSWORD_LENGTH)]
        [DefaultStringLength]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmación Password")]
        [Compare("Password", ErrorMessage = @"El password y su confirmación no coinciden.")]
        [MinLength(MembershipService.MIN_REQUIRED_PASSWORD_LENGTH)]
        [DefaultStringLength]
        public string ConfirmacionPassword { get; set; }

        [Required]
        [DisplayName("Telefono")]
        public Telefono Telefono { get; set; }

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

        [UIHint("DropDownList")]
        [DisplayName("Obra Social")]
        [DropDownProperty("ObraSocialId")]
        public IEnumerable<SelectListItem> ObrasSocialesHabilitadas { get; set; }

        [DisplayName("Plan")]
        [UIHint("CascadingDropDownList")]
        [CascadingDropDownPropertyAttribute("PlanId", "ObraSocialId", "GetPlanesObraSocial", "Domain", "Admin" ,"obraSocialId", "<< Seleccione una Obra Social >>")]
        public IEnumerable<SelectListItem> PlanesObraSocialHabilitados { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public virtual long? PlanId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public virtual long? ObraSocialId { get; set; }

        [Required]
        [DisplayName("Número Afiliado")]
        [DefaultStringLength]
        public virtual string NumeroAfiliado { get; set; }
    }
}