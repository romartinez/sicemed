using System.Web.Security;
using Sicemed.Web.Models;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web.Services.ApplicationServices.Cuenta
{
    public interface IMembershipApplicationService
    {
        int LargoMinimoPassword { get; }
        bool ValidarUsuario(string nombreUsuario, string password);
        MembershipCreateStatus CrearUsuario(string nombreUsuario, string password, string email);
        bool CambiarPassword(string nombreUsuario, string passwordViejo, string passwordNuevo);
        Usuario GetCurrentUser();
    }
}