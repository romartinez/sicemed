using System.Web;
using NHibernate.Impl;
using log4net;

namespace Sicemed.Web.Infrastructure.HttpModules
{
    public class LoggingThreadContextSetupModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, args) =>
                                        {
                                            ThreadContext.Properties["sessionId"] = GetSessionId();
                                            ThreadContext.Properties["userId"] = GetUserId(context);
                                            ThreadContext.Properties["userIp"] = GetUserIp(context);
                                            ThreadContext.Properties["rawUrl"] = GetRawUrl(context);
                                            ThreadContext.Properties["referrerUrl"] = GetReferrerUlr(context);
                                        };
        }

        private string GetReferrerUlr(HttpApplication context)
        {
            if (context.Context.Request.UrlReferrer == null) return string.Empty;
            return context.Context.Request.UrlReferrer.ToString();
        }

        private string GetRawUrl(HttpApplication context)
        {
            return context.Context.Request.RawUrl;
        }

        private string GetUserIp(HttpApplication context)
        {
            return context.Context.Request.UserHostAddress;
        }

        private string GetUserId(HttpApplication context)
        {
            if (context.Context.User == null)
                return "No User";
            if (!context.Context.User.Identity.IsAuthenticated)
                return "No Authenticated";
            return context.Context.User.Identity.Name;
        }

        private string GetSessionId()
        {
            return SessionIdLoggingContext.SessionId.ToString();
        }

        public void Dispose()
        {

        }
    }
}