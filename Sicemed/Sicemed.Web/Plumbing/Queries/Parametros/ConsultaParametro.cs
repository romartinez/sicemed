using System;
using NHibernate;
using Sicemed.Web.Models;

namespace Sicemed.Web.Plumbing.Queries.Parametros
{
    public interface IConsultaParametro : IQuery
    {
        T Execute<T>(Parametro.Keys key);
    }

    public class ConsultaParametro: IConsultaParametro
    {
        private readonly ISessionFactory _sessionFactory;

        public ConsultaParametro(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public virtual T Execute<T>(Parametro.Keys key)
        {
            using(var session = _sessionFactory.OpenStatelessSession())
            {
                var recovered = session.QueryOver<Parametro>().Where(p => p.Key == key.ToString()).SingleOrDefault();
                if (recovered == default(Parametro)) return default(T);
                return (T)Convert.ChangeType(recovered.Value, typeof(T));                
            }
        }
    }
}