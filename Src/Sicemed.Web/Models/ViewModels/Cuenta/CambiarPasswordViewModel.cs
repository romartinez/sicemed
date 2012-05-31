using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Validation;

namespace Sicemed.Web.Models.ViewModels.Cuenta
{
    public class CambiarPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password Actual")]
        public string PasswordActual { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "Password Nuevo")]
        public string PasswordNuevo { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmaci�n Password Nuevo")]
        [Compare("PasswordNuevo", ErrorMessage = @"El password nuevo y su confirmaci�n no coinciden.")]
        public string ConfirmacionPasswordNuevo { get; set; }
    }
}