using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Plumbing.Attributes
{
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public virtual Rol Rol { get; set; }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var usuario = (Usuario) filterContext.HttpContext.User;
            if (usuario.Roles.Contains(Rol)) ;
        }
    }
}