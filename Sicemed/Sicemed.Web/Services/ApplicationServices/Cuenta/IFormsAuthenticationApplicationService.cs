namespace Sicemed.Web.Services.ApplicationServices.Cuenta
{
    public interface IFormsAuthenticationApplicationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }
}