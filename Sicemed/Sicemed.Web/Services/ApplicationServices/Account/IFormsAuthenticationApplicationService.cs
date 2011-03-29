using Sicemed.Web.Models;

namespace Sicemed.Web.Services.ApplicationServices.Account
{
    public interface IFormsAuthenticationApplicationService
    {
        void SignIn(PrincipalBase principal);
        void SignOut();
    }
}