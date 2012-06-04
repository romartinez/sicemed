using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web;
using Castle.Core.Logging;
using Sicemed.Web.Infrastructure.Services;

namespace Sicemed.Web.Infrastructure.Providers.Cache
{
    public class InMemoryCacheProvider : ICacheProvider
    {
        public virtual ILogger Logger { get; set; }
        public virtual IMembershipService MembershipService { get; set; }

        #region ICacheProvider Members

        public T GetUserContext<T>(string key)
        {
            key = GetUserContextKey(key);
            return Get<T>(key);
        }

        public void AddUserContext(string key, object obj)
        {
            key = GetUserContextKey(key);
            Add(key, obj);
        }

        public void RemoveUserContext(string key, object obj)
        {
            key = GetUserContextKey(key);
            Remove(key);
        }

        public T Get<T>(string key)
        {
            if (HttpContext.Current == null) throw new ProviderException("The aren't a HttpContext available.");
            return (T) HttpContext.Current.Cache[key];
        }

        public void Add(string key, object obj)
        {
            if (HttpContext.Current == null) throw new ProviderException("The aren't a HttpContext available.");
            HttpContext.Current.Cache.Insert(key, obj, null, GetTiempoExpiracionCache(), TimeSpan.Zero);
        }

        public void Remove(string key)
        {
            if (HttpContext.Current == null) throw new ProviderException("The aren't a HttpContext available.");
            HttpContext.Current.Cache.Remove(key);
        }

        #endregion

        private string GetUserContextKey(string key)
        {
            var user = MembershipService.GetCurrentUser();
            var userKey = user == null ? "anonymous" : user.Membership.Email;
            return string.Format("{0}|{1}", userKey, key);
        }

        private DateTime GetTiempoExpiracionCache()
        {
            const string CONFIG_KEY = "TiempoExpiracionCache";
            const int DEFAULT_TIEMPO = 5;
            DateTime tiempoExpiracion;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[CONFIG_KEY]))
            {
                try
                {
                    var minutes = int.Parse(ConfigurationManager.AppSettings[CONFIG_KEY]);
                    //Cargamos el valor seteado
                    tiempoExpiracion = DateTime.Now.AddMinutes(minutes);
                } catch (Exception ex)
                {
                    //Loggeamos la excepcion
                    if (Logger.IsWarnEnabled)
                    {
                        Logger.WarnFormat(
                            "Se produjo un error al leer de las configuraciones la variable:[{0}={1}]; se esperaba un número entero. Exc: {2}",
                            CONFIG_KEY, ConfigurationManager.AppSettings[CONFIG_KEY], ex);
                    }
                    //Usamos un valor por defecto
                    tiempoExpiracion = DateTime.Now.AddMinutes(DEFAULT_TIEMPO);
                }
            } else
            {
                //Usamos un valor por defecto
                tiempoExpiracion = DateTime.Now.AddMinutes(DEFAULT_TIEMPO);
            }
            //Devolvemos la fecha limite de expiracion de cache
            return tiempoExpiracion;
        }
    }
}