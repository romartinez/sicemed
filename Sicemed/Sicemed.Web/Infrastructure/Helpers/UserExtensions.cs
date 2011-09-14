// ReSharper disable CheckNamespace

using System.Linq;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components.Roles;

namespace System.Security.Principal
// ReSharper restore CheckNamespace
{
    public static class UserExtensions
    {
        public static bool IsInRole(this IPrincipal user, Rol rol)
        {
            if (rol == null) throw new ArgumentNullException("rol");
            
            //Si no esta autenticado, no pertenece a los roles custom.
            if (!user.Identity.IsAuthenticated) return false;
            
            var persona = user as Persona;
            if (persona == null) throw new ArgumentException(@"El usuario debe ser del tipo Persona.", "user");

            return persona.Roles.Any(r=> r.Value == rol.Value);
        }
    }
}