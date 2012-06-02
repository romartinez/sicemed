using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Services;

namespace Sicemed.Web.Models.ViewModels.Cuenta
{
    public class CambiarPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password Actual")]
        public string PasswordActual { get; set; }

        [Required]
		[StringLength(MembershipService.MIN_REQUIRED_PASSWORD_LENGTH)]
        [DataType(DataType.Password)]
        [Display(Name = "Password Nuevo")]
        public string PasswordNuevo { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmación Password Nuevo")]
        [Compare("PasswordNuevo", ErrorMessage = @"El password nuevo y su confirmación no coinciden.")]
        public string ConfirmacionPasswordNuevo { get; set; }
    }
}