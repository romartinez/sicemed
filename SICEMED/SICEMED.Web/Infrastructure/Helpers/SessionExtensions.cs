using NHibernate;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class SessionExtensions
    {
        public static void Delete<TEntity>(this ISession session, object id)
        {
            var queryString = string.Format("delete {0} where id = :id",
                                            typeof(TEntity));
            session.CreateQuery(queryString)
                   .SetParameter("id", id)
                   .ExecuteUpdate();
        }
    }
}