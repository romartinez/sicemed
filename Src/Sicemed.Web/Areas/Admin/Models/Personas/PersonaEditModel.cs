using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Infrastructure.ModelBinders;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Areas.Admin.Models.Personas
{
    public class PersonaEditModel : ICustomBindeableProperties
    {
        [HiddenInput(DisplayValue = false)]
        public virtual long? Id { get; set; }

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

        [Requerido]
        [DisplayName("Tipo Documento")]
        [ScaffoldColumn(false)]
        public virtual int TipoDocumentoId { get; set; }

        [Requerido]
        [DisplayName("Número Documento")]
        public virtual long DocumentoNumero { get; set; }

        [Display(Name = "Fecha Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}", NullDisplayText = "")]
        [DisplayName("Peso (kg)")]
        public decimal? Peso { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}", NullDisplayText = "")]
        [DisplayName("Altura (cm)")]
        public decimal? Altura { get; set; }

        [Requerido]
        [Correo]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [LargoCadenaPorDefecto]
        public string Email { get; set; }

        [Requerido]
        [DisplayName("Telefono")]
        public Telefono Telefono { get; set; }

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
        [CascadingDropDownProperty("DomicilioLocalidadId", "DomicilioLocalidadProvinciaId", "GetLocalidades", "Domain", "Admin","provinciaId", "<< Seleccione una Provincia >>")]
        public IEnumerable<SelectListItem> LocalidadesHabilitadas { get; set; }

        [Requerido]
        [DisplayName("Localidad")]
        [ScaffoldColumn(false)]
        public virtual long? DomicilioLocalidadId { get; set; }

        [Requerido]
        [DisplayName("Provincia")]
        [ScaffoldColumn(false)]
        public virtual long? DomicilioLocalidadProvinciaId { get; set; }

        [DisplayName("Es Paciente")]
        public bool EsPaciente { get; set; }

        [DisplayName("Paciente")]
        public PacienteEditModel Paciente { get; set; }

        [DisplayName("Es Secretaria")]
        public bool EsSecretaria { get; set; }
        
        [DisplayName("Secretaria")]
        public SecretariaEditModel Secretaria { get; set; }

        [DisplayName("Es Profesional")]
        public bool EsProfesional { get; set; }

        [DisplayName("Profesional")]
        public ProfesionalEditModel Profesional { get; set; }

        [DisplayName("Es Administrador")]
        public bool EsAdmin { get; set; }        

        public PersonaEditModel()
        {
            Paciente = new PacienteEditModel();
            Secretaria = new SecretariaEditModel();
            Profesional = new ProfesionalEditModel();
        }

        public bool SkipProperty(PropertyDescriptor propertyDescriptor)
        {
            return (!EsPaciente && propertyDescriptor.PropertyType == typeof(PacienteEditModel))
                || (!EsSecretaria && propertyDescriptor.PropertyType == typeof(SecretariaEditModel))
                || (!EsProfesional && propertyDescriptor.PropertyType == typeof(ProfesionalEditModel));
        }
    }
}