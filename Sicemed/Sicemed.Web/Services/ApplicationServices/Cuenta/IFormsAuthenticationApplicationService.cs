namespace Sicemed.Web.Services.ApplicationServices.Cuenta
{
    public interface IFormsAuthenticationApplicationService
    {
        void IniciarSesion(string nombreUsuario, bool recordarme);
        void CerrarSesion();
    }
}