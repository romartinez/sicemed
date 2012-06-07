using System.Collections.Generic;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Paginas
{
    public interface IObtenerPaginasRaizQuery : IQuery<IEnumerable<Pagina>> { }

    public class ObtenerPaginasRaizQuery : Query<IEnumerable<Pagina>>, IObtenerPaginasRaizQuery
    {
        #region Implementation of IQuery<Pagina>

        protected override IEnumerable<Pagina> CoreExecute()
        {
            return SessionFactory.GetCurrentSession().QueryOver<Pagina>().Where(x => x.Padre == null).List();
        }

        #endregion
    }
}