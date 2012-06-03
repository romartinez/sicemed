using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Models.ViewModels.Cuenta
{
    public class RecuperarPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}