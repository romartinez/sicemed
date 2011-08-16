using System.Collections.Generic;
using NHibernate;

namespace SICEMED.Web.Infrastructure.SessionManagement
{
    public interface ISessionFactoryProvider
    {
        IEnumerable<ISessionFactory> GetSessionFactories();
    }
}