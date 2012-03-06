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
                var obj = session.Load<TEntity>(id);

                session.Delete(obj);
            } catch (GenericADOException ex)
            {
                if (ex.InnerException != null
                    && ex.InnerException is SqlException
                    && ex.InnerException.Data["HelpLink.EvtID"] != null
                    && ex.InnerException.Data["HelpLink.EvtID"].ToString() == "547")
                {
                    throw new ValidationErrorException(
                        "No se puede eliminar la entidad ya que hay otras entidades asociadas a esta. Elimine las dem�s entidades y luego proceda a eliminarla.");
                }
                throw;
            }
        }
    }
}