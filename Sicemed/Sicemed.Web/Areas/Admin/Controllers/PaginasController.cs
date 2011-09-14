using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class PaginasController : CrudBaseController<Pagina>
    {
        protected override Expression<Func<Pagina, object>> DefaultOrderBy
        {
            get { return x => x.Nombre; }
        }

        protected override Pagina AgregarReferencias(Pagina modelo)
        {
            var padreId = RetrieveParameter<long>("padreId", "Padre", true);
            var session = SessionFactory.GetCurrentSession();

            if(padreId != default(long))
            {
                var paginaPadre = session.Get<Pagina>(padreId);

                modelo.Padre = paginaPadre;
            }

            return modelo;
        }

        public virtual ActionResult ObtenerPaginasPadre()
        {
            using (var session = SessionFactory.OpenStatelessSession())
            {
                ViewData.Model = session.QueryOver<Pagina>()                    
                    .Where(x => x.Padre == null).List();

                return PartialView();
            }
        }
    }
}
