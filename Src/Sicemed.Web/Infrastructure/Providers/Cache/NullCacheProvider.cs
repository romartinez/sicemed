namespace Sicemed.Web.Infrastructure.Providers.Cache
{
    public class NullCacheProvider : ICacheProvider
    {
        public static ICacheProvider Instance = new NullCacheProvider();

        #region ICacheProvider Members

        public T GetUserContext<T>(string key)
        {
            return Get<T>(key);
        }

        public void AddUserContext(string key, object obj) { }
        public void RemoveUserContext(string key) { }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public void Add(string key, object obj) { }
        public void Remove(string key) { }

        #endregion
    }
}