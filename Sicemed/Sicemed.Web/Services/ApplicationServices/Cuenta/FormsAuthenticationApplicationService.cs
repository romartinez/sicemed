using System;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Sicemed.Web.Models;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web.Services.ApplicationServices.Cuenta
{
    public class FormsAuthenticationApplicationService : IFormsAuthenticationApplicationService
    {
        #region IFormsAuthenticationApplicationService Members

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public FormsAuthenticationApplicationService()
        {
            _jsonSerializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        }

        public void IniciarSesion(Usuario usuario, bool recordarme = false)
        {
            Check.Require(usuario != default(Usuario));
            
            var authTicket = new FormsAuthenticationTicket(1,
                                                           usuario.NombreUsuario,
                                                           DateTime.Now,
                                                           DateTime.Now.AddMinutes(15),
                                                           recordarme,
                                                           usuario.Id.ToString());
            var encTicket = FormsAuthentication.Encrypt(authTicket);
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