using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Infrastructure.Services;

namespace Sicemed.Web.Models.ViewModels.Cuenta
{
    public class CambiarPasswordViewModel
    {
        [Requerido]
        [DataType(DataType.Password)]
        [Display(Name = "Password Actual")]
        public string PasswordActual { get; set; }

        [Requerido]
		[LargoCadenaMinimo(MembershipService.MIN_REQUIRED_PASSWORD_LENGTH)]
        [DataType(DataType.Password)]
        [Display(Name = "Password Nuevo")]
        public string PasswordNuevo { get; set; }

        [DataType(DataType.Password)]
        [LargoCadenaMinimo(MembershipService.MIN_REQUIRED_PASSWORD_LENGTH)]
        [Display(Name = "Confirmación Password Nuevo")]
        [Compare("PasswordNuevo", ErrorMessage = @"El password nuevo y su confirmación no coinciden.")]
        public string ConfirmacionPasswordNuevo { get; set; }
    }
}