using System;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web.Services.ApplicationServices.Cuenta
{
    public class FormsAuthenticationApplicationService : IFormsAuthenticationApplicationService
    {
        #region IFormsAuthenticationApplicationService Members

        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("Value cannot be null nor empty.", "userName");

            // Create and tuck away the cookie
            //string serializedIdentity = JsonConvert.SerializeObject(principal.IdentityBase.ExtendedData);
            var authTicket = new FormsAuthenticationTicket(1,
                                                           userName,
                                                           DateTime.Now,
                                                           DateTime.Now.AddMinutes(15),
                                                           createPersistentCookie, 
                                                           string.Empty);//serializedIdentity);
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