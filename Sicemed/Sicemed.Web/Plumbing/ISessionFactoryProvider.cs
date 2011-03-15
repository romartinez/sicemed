using System.Collections.Generic;
using NHibernate;

namespace Sicemed.Web.Plumbing
{
    public interface ISessionFactoryProvider
    {
        IEnumerable<ISessionFactory> GetSessionFactories();
    }
}