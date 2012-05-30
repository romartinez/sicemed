using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace System.Security.Principal
// ReSharper restore CheckNamespace
{
    public static class UserExtensions
    {
        public static bool IsInRole<T>(this IPrincipal user) where T : Rol
        {
            //Si no esta autenticado, no pertenece a los roles custom.
            if (!user.Identity.IsAuthenticated) return false;

            var persona = user as Persona;
            if (persona == null) throw new ArgumentException(@"El usuario debe ser del tipo Persona.", "user");

            return persona.IsInRole<T>();
        }
    }
}