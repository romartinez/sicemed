using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class MenuAttribute : NHibernateBaseAttribute
    {
        public override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            var session = SessionFactory.GetCurrentSession();
            
            var paginas = session.QueryOver<Pagina>().Fetch(x => x.Hijos).Eager.Where(x => x.Padre == null).Future();
            filterContext.Controller.ViewData["_Menu"] = paginas;            
        }
    }
}