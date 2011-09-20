using System.Collections.Generic;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Paginas
{
    public interface IObtenerPaginasRaizQuery : IQuery<Pagina> {}

    public class ObtenerPaginasRaizQuery : Query<Pagina>, IObtenerPaginasRaizQuery
    {
        #region Implementation of IQuery<Pagina>

        public override IEnumerable<Pagina> CoreExecute()
        {
            return SessionFactory.GetCurrentSession().QueryOver<Pagina>().Where(x => x.Padre == null).List();
        }

        #endregion
    }
}