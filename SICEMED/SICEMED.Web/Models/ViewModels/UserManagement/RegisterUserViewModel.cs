using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Validation;
using Sicemed.Web.Models.Enumerations;

namespace Sicemed.Web.Models.ViewModels.UserManagement
{
    public class RegisterUserViewModel
    {
        [Required]    
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Retry Password")]
        [Compare("Password")]
        public string PasswordRetry { get; set; }

        [Required]
        [Email]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public long[] SelectedRoles { get; set; }

        public List<Rol> AllRoles { get; set; }

        public RegisterUserViewModel()
        {
            AllRoles = new List<Rol>();
            SelectedRoles = new long[]{};
        }

        public RegisterUserViewModel(Usuario user) : this()
        {
            FullName = user.Nombre;
            Email = user.Membership.Email;
            SelectedRoles = user.Roles.Select(r => r.Rol.Value).ToArray();
        }
    }
}