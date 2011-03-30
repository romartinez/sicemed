using Sicemed.Web.Models;

namespace Sicemed.Web.Services.ApplicationServices.Cuenta
{
    public interface IFormsAuthenticationApplicationService
    {
        void IniciarSesion(Usuario usuario, bool recordarme = false);
        void CerrarSesion();
    }
}