using System.Web;
using Sicemed.Web.Models;

namespace Sicemed.Web.ApplicationServices.Account {
    public interface IUserDiscoveringApplicationService {
        PrincipalBase GetCurrentUser();
    }

    public class UserDiscoveringApplicationService : IUserDiscoveringApplicationService {
        public PrincipalBase GetCurrentUser() {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return null;
            return (PrincipalBase)HttpContext.Current.User;
        }
    }
}