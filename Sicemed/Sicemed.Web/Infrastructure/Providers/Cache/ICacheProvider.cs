namespace Sicemed.Web.Infrastructure.Providers.Cache
{
    public interface ICacheProvider
    {
        T GetUserContext<T>(string key);
        void AddUserContext(string key, object obj);
        T Get<T>(string key);
        void Add(string key, object obj);
    }
}