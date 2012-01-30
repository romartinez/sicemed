using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Transform;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class MenuAttribute : NHibernateBaseAttribute
    {
        private IMembershipService _membershipService;

        public MenuAttribute(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        private dynamic Convert(Pagina pagina, int i, int total, dynamic padre = null)
        {
            dynamic paginaDyn = new ExpandoObject();
            paginaDyn.Hijos = new List<dynamic>();

            paginaDyn.Nombre = pagina.Nombre;
            paginaDyn.Url = VirtualPathUtility.ToAbsolute("~/") + pagina.Url;
            paginaDyn.IsParent = pagina.Hijos.Any();
            paginaDyn.IsCurrent = false;
            paginaDyn.IsCurrentItem = false;
            paginaDyn.IsFirst = (i == 0);
            paginaDyn.IsLast = (i > 0 && i == total - 1);
            paginaDyn.Parent = padre;

            var hijosCount = pagina.Hijos.Count;
            for (var j = 0; j < hijosCount; j++)
            {
                paginaDyn.Hijos.Add(Convert(pagina.Hijos.ElementAt(j), j, hijosCount, paginaDyn));
            }

            return paginaDyn;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var url = VirtualPathUtility.ToAbsolute("~/")
                + filterContext.RouteData.Route.GetVirtualPath(filterContext.RequestContext,
                                                                                  filterContext.RouteData.Values).VirtualPath;

            var session = SessionFactory.GetCurrentSession();
            var paginasDb = session.QueryOver<Pagina>()
                                .Fetch(x => x.Hijos).Eager
                                .OrderBy(x => x.Orden).Asc
                                .Where(x => x.Padre == null)
                                .TransformUsing(Transformers.DistinctRootEntity).Future();

            var paginas = new List<dynamic>();
            var paginasCount = paginasDb.Count();
            for (var i = 0; i < paginasCount; i++)
            {
                var pagina = paginasDb.ElementAt(i);
                paginas.Add(Convert(pagina, i, paginasCount));
            }

            var user = _membershipService.GetCurrentUser();
            if (user != null && user.IsInRole(Rol.ADMINISTRADOR))
            {
                dynamic pagina = new ExpandoObject();
                pagina.Url = "";
                pagina.Nombre = "Admin";
                pagina.Hijos = new List<dynamic>();
                pagina.IsParent = true;
                pagina.IsCurrent = false;
                pagina.IsCurrentItem = false;
                pagina.IsFirst = false;
                pagina.IsLast = true;
                pagina.Parent = null;

                dynamic hijo = new ExpandoObject();
                hijo.Url = VirtualPathUtility.ToAbsolute("~/") + "Admin/Clinicas";
                hijo.Nombre = "Clinica";
                hijo.Hijos = new List<dynamic>();
                hijo.IsParent = false;
                hijo.IsCurrent = false;
                hijo.IsCurrentItem = false;
                hijo.IsFirst = true;
                hijo.IsLast = false;
                hijo.Parent = pagina;

                pagina.Hijos.Add(hijo);

                paginas.Add(pagina);
            }

            //Actualizamos cual es el current
            var currentPagina = GetByUrl(paginas, url);

            if (currentPagina != null)
            {
                MarkAsCurrent(currentPagina);
            }


            filterContext.Controller.ViewData["_Menu"] = paginas;
        }

        private dynamic GetByUrl(List<dynamic> paginas, string url)
        {
            foreach (var pagina in paginas)
            {
                if (pagina.Url == url) return pagina;
                if (pagina.IsParent)
                {
                    var encontrada = GetByUrl(pagina.Hijos, url);
                    if (encontrada != null) return encontrada;
                }
            }
            return null;
        }


        private void MarkAsCurrent(dynamic pagina, bool isCurrentItem = true)
        {
            pagina.IsCurrent = true;
            pagina.IsCurrentItem = isCurrentItem;
            if (pagina.Parent != null) MarkAsCurrent(pagina.Parent, false);
        }
    }
}