using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;
using Sicemed.Web.Plumbing.Attributes;

namespace Sicemed.Web.Areas.Public.Models.Cuenta
{
    public class CambiarPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password Actual")]
        public string PasswordViejo { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "Nuevo Password")]
        public string PasswordNuevo { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme Nuevo Password")]
        [Compare("Password",
            ErrorMessageResourceName = "VALIDATOR_PASSWORD_PASSWORDCONFIRMATION_NOT_EQUALS",
            ErrorMessageResourceType = typeof(Recursos))]
        public string PasswordNuevoConfirmacion { get; set; }
    }
}