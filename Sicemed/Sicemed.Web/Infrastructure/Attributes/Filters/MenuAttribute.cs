using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Transform;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Providers.Cache;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels.Menu;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class MenuAttribute : NHibernateBaseAttribute
    {
        private const string CACHE_KEY = "MENU_PAGES";

        private readonly IMembershipService _membershipService;
        private readonly ICacheProvider _cacheProvider;

        public MenuAttribute(IMembershipService membershipService, ICacheProvider cacheProvider)
        {
            _membershipService = membershipService;
            _cacheProvider = cacheProvider;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var currentUser = _membershipService.GetCurrentUser();

            //Check the cache for the menu
            var pages = _cacheProvider.GetUserContext<List<PageViewModel>>(CACHE_KEY);
            if (pages == null)
            {
                var session = SessionFactory.GetCurrentSession();
                var pagesFromDb = session.QueryOver<Pagina>()
                    .Fetch(x => x.Hijos).Eager
                    .Where(x => x.Padre == null)
                    .TransformUsing(Transformers.DistinctRootEntity)
                    .Future();

                pages = new List<PageViewModel>();

                pagesFromDb.ToList().ForEach(x => pages.Add(ConvertModelToDynamic(x)));

                AttachCorePages(pages);

                if (currentUser != null && currentUser.IsInRole(Rol.ADMINISTRADOR))
                {
                    AttachAdminPages(pages);
                }

                pages = FixHierarchy(pages);

                _cacheProvider.AddUserContext(CACHE_KEY, pages);
            }

            //Set the current Page
            var virtualPathData = filterContext.RouteData.Route.GetVirtualPath(filterContext.RequestContext, filterContext.RouteData.Values);
            if (virtualPathData == null) throw new NullReferenceException("The virtual path for the current request couldn't be retrieved");

            var currentUrl = VirtualPathUtility.ToAbsolute("~/") + virtualPathData.VirtualPath;

            var clonedPages = pages.Clone(); 
            var currentPage = FinmdByUrl(clonedPages, currentUrl);
            if (currentPage != null)
            {
                MarkPageAndParentAsCurrent(currentPage);
            }

            //Leave it for the view
            filterContext.Controller.ViewData["_Menu"] = clonedPages;
        }

        #region Helper Private Methods


        private static PageViewModel ConvertModelToDynamic(Pagina pagina, PageViewModel parent = null)
        {
            var page = new PageViewModel
                           {
                               Name = pagina.Nombre,
                               Url = VirtualPathUtility.ToAbsolute("~/") + pagina.Url,
                               Parent = parent,
                               Order = pagina.Orden
                           };

            var childCount = pagina.Hijos.Count;
            for (var i = 0; i < childCount; i++)
            {
                page.Childs.Add(ConvertModelToDynamic(pagina.Hijos.ElementAt(i), page));
            }

            return page;
        }

        private static void AttachCorePages(ICollection<PageViewModel> pages)
        {
            pages.Add(CreateDefaultPage("Obtener Turno", "ObtenerTurno", order: 9990)); //Almost at the end
        }

        private static void AttachAdminPages(ICollection<PageViewModel> pages)
        {
            var adminPage = CreateDefaultPage("Admin", "#", order: 9999); //Show last in the menu.

            adminPage.Childs.Add(CreateDefaultPage("Clinica", "Admin/Clinicas", adminPage));
            adminPage.Childs.Add(CreateDefaultPage("Paginas", "Admin/Paginas", adminPage));
            adminPage.Childs.Add(CreateDefaultPage("Personas", "Admin/Personas", adminPage));
            adminPage.Childs.Add(CreateDefaultPage("Provincias", "Admin/Provincias", adminPage));
            adminPage.Childs.Add(CreateDefaultPage("Localidades", "Admin/Localidades", adminPage));
            adminPage.Childs.Add(CreateDefaultPage("Feriados", "Admin/Feriados", adminPage));
            adminPage.Childs.Add(CreateDefaultPage("Obras Sociales", "Admin/ObrasSociales", adminPage));
            adminPage.Childs.Add(CreateDefaultPage("Planes", "Admin/Planes", adminPage));
            adminPage.Childs.Add(CreateDefaultPage("Especialidades", "Admin/Especialidades", adminPage));
            adminPage.Childs.Add(CreateDefaultPage("Consultorios", "Admin/Consultorios", adminPage));

            pages.Add(adminPage);
        }

        private static List<PageViewModel> FixHierarchy(ICollection<PageViewModel> pages)
        {
            var orderedPages = pages.OrderBy(x => x.Order).ToList();
            for (var i = 0; i < orderedPages.Count; i++)
            {
                var page = orderedPages[i];
                page.IsFirst = i == 0;
                page.IsLast = (i > 0 && i == pages.Count - 1);
                page.IsCurrent = false;
                page.IsCurrentItem = false;

                if (page.IsParent)
                {
                    page.Childs = FixHierarchy(page.Childs);
                }
            }

            return orderedPages;
        }

        private static PageViewModel CreateDefaultPage(string name = "", string url = "", PageViewModel parent = null, int order = 0)
        {
            var page = new PageViewModel
                                  {
                                      Url = VirtualPathUtility.ToAbsolute("~/") + url,
                                      Name = name,
                                      Parent = parent,
                                      Order = order
                                  };

            return page;
        }

        private static PageViewModel FinmdByUrl(IEnumerable<PageViewModel> pages, string url)
        {
            foreach (var page in pages)
            {
                if (page.Url == url) return page;
                if (page.IsParent)
                {
                    var found = FinmdByUrl(page.Childs, url);
                    if (found != null) return found;
                }
            }
            return null;
        }

        private static void MarkPageAndParentAsCurrent(PageViewModel pagina, bool isCurrentItem = true)
        {
            pagina.IsCurrent = true;
            pagina.IsCurrentItem = isCurrentItem;
            if (pagina.Parent != null) MarkPageAndParentAsCurrent(pagina.Parent, false);
        }

        #endregion
    }
}