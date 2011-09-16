using System.Web.Mvc;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class MenuAttribute : NHibernateBaseAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = SessionFactory.GetCurrentSession();

            var paginas = session.QueryOver<Pagina>().Fetch(x => x.Hijos).Eager.Where(x => x.Padre == null).Future();
            filterContext.Controller.ViewData["_Menu"] = paginas;
        }
    }
}