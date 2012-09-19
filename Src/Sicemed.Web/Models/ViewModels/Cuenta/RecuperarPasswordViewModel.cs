using System.ComponentModel.DataAnnotations;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models.ViewModels.Cuenta
{
    public class RecuperarPasswordViewModel
    {
        [Requerido]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}