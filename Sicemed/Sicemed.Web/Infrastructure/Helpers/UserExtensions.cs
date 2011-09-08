// ReSharper disable CheckNamespace

using System.Linq;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations.Roles;

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
            
            var usuario = user as Usuario;
            if (usuario == null) throw new ArgumentException(@"El usuario debe ser del tipo Usuario.", "user");

            return usuario.Roles.Any(r=> r.Rol.Value == rol.Value);
        }
    }
}