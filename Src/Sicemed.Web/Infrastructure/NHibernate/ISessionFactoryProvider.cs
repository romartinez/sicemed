using System.Collections.Generic;
using NHibernate;

namespace Sicemed.Web.Infrastructure.NHibernate
{
    public interface ISessionFactoryProvider
    {
        IEnumerable<ISessionFactory> GetSessionFactories();
    }
}