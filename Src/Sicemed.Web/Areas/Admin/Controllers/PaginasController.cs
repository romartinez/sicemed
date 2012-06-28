using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using SICEMED.Web;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
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

            if (padreId != default(long))
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

        protected override bool EsValido(Pagina modelo)
        {
            //Valido que el nombre sea unico.
            var session = SessionFactory.GetCurrentSession();
            
            var paginaConMismoNombre =
                session.QueryOver<Pagina>().Where(x => x.Nombre == modelo.Nombre && x.Id != modelo.Id).Future();

            var paginaConMismaUrl =
                session.QueryOver<Pagina>().Where(x => x.Url == modelo.Url && x.Id != modelo.Id).Future();

            if (!modelo.IsTransient())
            {
                //Lo busco y traigo los hijos, si tiene hijos no puede tener padre; un nivel de profundidad.
                var padreConHijos =
                    session.QueryOver<Pagina>().Where(x => x.Id == modelo.Id).Fetch(x => x.Hijos).Eager.Future().First();
                if (modelo.Padre != null && padreConHijos.Hijos.Any())
                    throw new ValidationErrorException(
                        "Se admite un único nivel de profundidad. La página no puede poseer hijos y tener un padre al mismo tiempo. Quitele el padre.");
            }

            if (paginaConMismoNombre.Any())
                throw new ValidationErrorException(string.Format("Ya existe una página con el Nombre: '{0}'",
                                                                 modelo.Nombre));
            if (paginaConMismaUrl.Any())
                throw new ValidationErrorException(string.Format("Ya existe una página con la misma Url: '{0}'",
                                                                 modelo.Url));

            return base.EsValido(modelo);
        }

        [ValidateInput(false)]
        public override void Nuevo(string oper, Pagina modelo, int paginaId = 0)
        {
            base.Nuevo(oper, modelo, paginaId);
            MvcApplication.ResetRoutes();
            Cache.RemoveUserContext(MenuAttribute.CacheKey);
        }

        [ValidateInput(false)]
        public override void Editar(long id, string oper, Pagina modelo)
        {
            base.Editar(id, oper, modelo);
            MvcApplication.ResetRoutes();
            Cache.RemoveUserContext(MenuAttribute.CacheKey);
        }

        public override void Eliminar(string id, string oper)
        {
            base.Eliminar(id, oper);
            MvcApplication.ResetRoutes();
            Cache.RemoveUserContext(MenuAttribute.CacheKey);
        }
    }
}