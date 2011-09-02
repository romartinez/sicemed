using System;
using System.Web;
using System.Web.Security;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.HttpModules
{
    public class SecurityAuthenticationModule : IHttpModule
    {
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += AuthenticateRequest;
        }

        public void Dispose() {}

        #endregion

        private void AuthenticateRequest(object sender, EventArgs e)
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = HttpContext.Current.Request.Cookies[cookieName];

            if (authCookie == null) return;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var id = Convert.ToInt64(authTicket.UserData);
            var session = ServiceLocator.Current.GetInstance<ISessionFactory>().GetCurrentSession();
            HttpContext.Current.User = session.Get<Usuario>(id);
        }
    }
}