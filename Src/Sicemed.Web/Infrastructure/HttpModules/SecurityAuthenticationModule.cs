using System;
using System.Web;
using System.Web.Security;
using Castle.Core.Logging;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using Sicemed.Web.Infrastructure.Providers.Cache;
using Sicemed.Web.Models;
using System.Linq;

namespace Sicemed.Web.Infrastructure.HttpModules
{
    public class SecurityAuthenticationModule : IHttpModule
    {
        public const string CACHE_KEY = "USER_CACHE_{0}";
        private ILogger _logger = ServiceLocator.Current.GetInstance<ILogger>();
        static string[] ExtensionsToSkip = new[] { ".js", ".css", ".ico", ".png", ".gif", ".jpg", ".woff" };

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += AuthenticateRequest;
        }

        public void Dispose() {}

        #endregion

        private void AuthenticateRequest(object sender, EventArgs e)
        {
            if (ExtensionsToSkip.Contains(HttpContext.Current.Request.CurrentExecutionFilePathExtension)) 
                return;
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = HttpContext.Current.Request.Cookies[cookieName];

            if (authCookie == null) return;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var id = Convert.ToInt64(authTicket.UserData);
            var cacheProvider = ServiceLocator.Current.GetInstance<ICacheProvider>();
            var cacheKey = string.Format(CACHE_KEY, id);
            HttpContext.Current.User = cacheProvider.Get<Persona>(cacheKey);
            if (HttpContext.Current.User != null) return;

            var session = ServiceLocator.Current.GetInstance<ISessionFactory>().GetCurrentSession();
            try
            {
                HttpContext.Current.User = session.Get<Persona>(id);
                cacheProvider.Add(cacheKey, HttpContext.Current.User);
            } 
            catch (Exception ex)
            {
                _logger.Fatal(string.Format("There was an error authenticating the user: '{0}'.", id), ex);
                FormsAuthentication.SignOut();
            }            
        }
    }
}