using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;
using Sicemed.Web.Plumbing.Attributes;

namespace Sicemed.Web.Areas.Public.Models.Cuenta
{
    public class RegistroViewModel
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessageResourceName = "VALIDADOR_PASSWORD_PASSWORD_CONFIRMACION_DIFERENTES",
            ErrorMessageResourceType = typeof(Recursos))]
        public string PasswordConfirmacion { get; set; }
    }
}