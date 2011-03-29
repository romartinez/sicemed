using Sicemed.Web.Models;

namespace Sicemed.Web.Services.ApplicationServices.Account
{
    public interface IUserDiscoveringApplicationService
    {
        PrincipalBase GetCurrentUser();
    }
}