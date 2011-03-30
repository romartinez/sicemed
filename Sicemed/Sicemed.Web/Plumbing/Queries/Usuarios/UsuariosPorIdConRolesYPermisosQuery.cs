using NHibernate;
using Sicemed.Web.Models;

namespace Sicemed.Web.Plumbing.Queries.Usuarios
{
    public interface IUsuariosPorIdConRolesYPermisosQuery : IQuery
    {
        Usuario Execute(long id);
    }

    public class UsuariosPorIdConRolesYPermisosQuery : IUsuariosPorIdConRolesYPermisosQuery
    {
        private readonly ISessionFactory _sessionFactory;

        public UsuariosPorIdConRolesYPermisosQuery(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }


        public virtual Usuario Execute(long id)
        {
            using(var session = _sessionFactory.OpenStatelessSession())
            {
                return session.QueryOver<Usuario>().Where(u=> u.Id == id).Fetch(x=>x.Roles).Eager.SingleOrDefault();
            }
        }
    }
}