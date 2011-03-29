using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Areas.Public.Models.Cuenta
{
    public class IniciarSesionViewModel
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool Recordarme { get; set; }
    }
}