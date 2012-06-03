using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Infrastructure.Services;

namespace Sicemed.Web.Models.ViewModels.Cuenta
{
    public class RegistroPersonaViewModel
    {
        [Required]
        [Display(Name = "Nombre", Prompt = "AAAA")]
        [DefaultStringLength]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        [DefaultStringLength]
        public string Apellido { get; set; }

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
    }
}