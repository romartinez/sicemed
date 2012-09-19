using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models.ViewModels.Cuenta
{
    public class InciarSesionViewModel
    {
        [Requerido]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [LargoCadenaPorDefecto]
        [Correo]
        public string Email { get; set; }

        [Requerido]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [LargoCadenaPorDefecto]
        public string Password { get; set; }

        [DisplayName("Recordarme?")]
        public bool RememberMe { get; set; }
    }
}