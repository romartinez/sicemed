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
            paginaDyn.IsCurrent = false;
            paginaDyn.IsCurrentItem = false;
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
                var admin = CreateDefaultPagina("Admin", "#");                
                var clinica = CreateDefaultPagina("Clinica", "Admin/Clinicas", admin);
                admin.Hijos.Add(clinica);

                admin.Hijos.Add(CreateDefaultPagina("Paginas", "Admin/Paginas", admin));
                admin.Hijos.Add(CreateDefaultPagina("Personas", "Admin/Personas", admin));
                admin.Hijos.Add(CreateDefaultPagina("Provincias", "Admin/Provincias", admin));
                admin.Hijos.Add(CreateDefaultPagina("Localidades", "Admin/Localidades", admin));
                admin.Hijos.Add(CreateDefaultPagina("Feriados", "Admin/Feriados", admin));
                admin.Hijos.Add(CreateDefaultPagina("Obras Sociales", "Admin/ObrasSociales", admin));
                admin.Hijos.Add(CreateDefaultPagina("Planes", "Admin/Planes", admin));
                admin.Hijos.Add(CreateDefaultPagina("Especialidades", "Admin/Especialidades", admin));
                admin.Hijos.Add(CreateDefaultPagina("Consultorios", "Admin/Consultorios", admin));

                paginas.Add(admin);
            }

            //Actualizamos cual es el current

            FixHierarchy(paginas);

            var currentPagina = GetByUrl(paginas, url);

            if (currentPagina != null)
            {
                MarkAsCurrent(currentPagina);
            }


            filterContext.Controller.ViewData["_Menu"] = paginas;
        }

        private void FixHierarchy(List<dynamic> pages)
        {
            for(var i = 0; i < pages.Count; i++)
            {
                var page = pages[i];
                page.IsFirst = i == 0;
                page.IsLast = (i > 0 && i == pages.Count - 1);
                page.IsParent = page.Hijos.Count > 0;

                if(page.IsParent) FixHierarchy(page.Hijos);
            }
        }


        private dynamic CreateDefaultPagina(string nombre = "", string url = "", dynamic padre = null)
        {
            dynamic dynamicPage = new ExpandoObject();
            dynamicPage.Url = VirtualPathUtility.ToAbsolute("~/") + url;
            dynamicPage.Nombre = nombre;
            dynamicPage.Hijos = new List<dynamic>();
            dynamicPage.IsCurrent = false;
            dynamicPage.IsCurrentItem = false;
            dynamicPage.Parent = padre;

            return dynamicPage;
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