using System.Web;
using Sicemed.Web.Models;

namespace Sicemed.Web.Services.ApplicationServices.Account
{
    public class UserDiscoveringApplicationService : IUserDiscoveringApplicationService
    {
        #region IUserDiscoveringApplicationService Members

        public PrincipalBase GetCurrentUser()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return null;
            return (PrincipalBase) HttpContext.Current.User;
        }

        #endregion
    }
}