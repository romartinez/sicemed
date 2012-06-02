using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using Sicemed.Web.Infrastructure.Services;

namespace Sicemed.Web.Models.ViewModels.Cuenta
{
    public class RegistroPersonaViewModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
		[Email]
        public string Email { get; set; }

        [Required]
		[StringLength(MembershipService.MIN_REQUIRED_PASSWORD_LENGTH)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmación Password")]
        [Compare("Password", ErrorMessage = @"El password y su confirmación no coinciden.")]
		[StringLength(MembershipService.MIN_REQUIRED_PASSWORD_LENGTH)]
		public string ConfirmacionPassword { get; set; }
    }
}