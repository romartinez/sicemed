using System;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Sicemed.Web.Models;

namespace Sicemed.Web.ApplicationServices.Account
{
    public interface IFormsAuthenticationApplicationService
    {
        void SignIn(PrincipalBase principal);
        void SignOut();
    }

    public class FormsAuthenticationApplicationService : IFormsAuthenticationApplicationService
    {
        #region IFormsAuthenticationApplicationService Members

        public void SignIn(PrincipalBase principal)
        {
            if (principal == null) throw new ArgumentException("Value cannot be null.", "principal");

            // Create and tuck away the cookie
            string serializedIdentity = JsonConvert.SerializeObject(principal.IdentityBase.ExtendedData);
            var authTicket = new FormsAuthenticationTicket(1,
                                                           principal.Identity.Name,
                                                           DateTime.Now,
                                                           DateTime.Now.AddMinutes(15),
                                                           false,
                                                           serializedIdentity);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        #endregion
    }
}