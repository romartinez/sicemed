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

        public void IniciarSesion(string nombreUsuario, bool recordarme)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario)) throw new ArgumentException("Value cannot be null nor empty.", "nombreUsuario");

            // Create and tuck away the cookie
            //string serializedIdentity = JsonConvert.SerializeObject(principal.IdentityBase.ExtendedData);
            var authTicket = new FormsAuthenticationTicket(1,
                                                           nombreUsuario,
                                                           DateTime.Now,
                                                           DateTime.Now.AddMinutes(15),
                                                           recordarme, 
                                                           string.Empty);//serializedIdentity);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }

        public void CerrarSesion()
        {
            FormsAuthentication.SignOut();
        }

        #endregion
    }
}