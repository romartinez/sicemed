using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.UI;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class PaginasController : CrudBaseController<Pagina>
    {
        [HttpGet]
        [AjaxHandleError]
        [OutputCache(Duration = 600, VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient)]//10 minutes(10min*60sec)
        [ValidateAntiForgeryToken]
        public ActionResult ObtenerPaginas()
        {
            var paginas = SessionFactory.GetCurrentSession().QueryOver<Pagina>().List();
            return Json(paginas);
        }

        protected override Expression<Func<Pagina, object>> DefaultOrderBy
        {
            get { return x => x.Nombre; }
        }
    }
}
