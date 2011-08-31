using System;
using System.Web;
using System.Web.Security;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public interface IFormAuthenticationStoreService
    {
        void CreateLogInCookie(Usuario user, bool rememberMe = false);
        void SingOut();
    }

    public class FormAuthenticationStoreService : IFormAuthenticationStoreService
    {
        #region IFormAuthenticationStoreService Members

        public void CreateLogInCookie(Usuario user, bool rememberMe = false)
        {
            var authTicket = new FormsAuthenticationTicket(1,
                                                           user.Membership.Email,
                                                           DateTime.Now,
                                                           DateTime.Now.AddMinutes(15),
                                                           rememberMe,
                                                           user.Id.ToString());
            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }

        public void SingOut()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                throw new ApplicationException("The user is not authenticated.");

            FormsAuthentication.SignOut();
        }

        #endregion
    }
}