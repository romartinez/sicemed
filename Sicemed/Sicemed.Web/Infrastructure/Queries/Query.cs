using System.Collections.Generic;
using NHibernate;

namespace Sicemed.Web.Infrastructure.Queries
{
    public abstract class Query<T> : IQuery<T>
    {
        public virtual ISessionFactory SessionFactory { get; set; }

        #region Implementation of IQuery<T>

        public abstract IEnumerable<T> Execute();

        #endregion
    }
}