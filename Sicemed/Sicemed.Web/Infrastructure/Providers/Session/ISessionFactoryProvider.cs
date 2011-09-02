using System.Collections.Generic;
using NHibernate;

namespace Sicemed.Web.Infrastructure.Providers.Session
{
    public interface ISessionFactoryProvider
    {
        IEnumerable<ISessionFactory> GetSessionFactories();
    }
}