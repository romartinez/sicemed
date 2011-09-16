using System.Data.SqlClient;
using NHibernate;
using NHibernate.Exceptions;
using Sicemed.Web.Infrastructure.Exceptions;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class SessionExtensions
    {
        public static void Delete<TEntity>(this ISession session, object id)
        {
            try
            {
                var queryString = string.Format("delete {0} where id = :id",
                                                typeof (TEntity));
                session.CreateQuery(queryString)
                    .SetParameter("id", id)
                    .ExecuteUpdate();
            } catch (GenericADOException ex)
            {
                if (ex.InnerException != null
                    && ex.InnerException is SqlException
                    && ex.InnerException.Data["HelpLink.EvtID"] != null
                    && ex.InnerException.Data["HelpLink.EvtID"].ToString() == "547")
                {
                    throw new ValidationErrorException(
                        "No se puede eliminar la entidad ya que hay otras entidades asociadas a esta. Elimine las demás entidades y luego proceda a eliminarla.");
                }
                throw;
            }
        }
    }
}