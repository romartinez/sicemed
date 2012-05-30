using System;
using System.Web;
using System.Web.Security;
using Castle.Core.Logging;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using Sicemed.Web.Models;
using log4net;

namespace Sicemed.Web.Infrastructure.HttpModules
{
    public class SecurityAuthenticationModule : IHttpModule
    {
        private ILogger _logger = ServiceLocator.Current.GetInstance<ILogger>();

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
            try
            {
                HttpContext.Current.User = session.Get<Persona>(id);   
            } 
            catch (Exception ex)
            {
                _logger.Fatal(string.Format("There was an error authenticating the user: '{0}'.", id), ex);
                FormsAuthentication.SignOut();
            }            
        }
    }
}