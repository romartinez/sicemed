using System;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using Sicemed.Web.Areas.Admin.Models.Auditoria;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    [AuthorizeIt(typeof(Administrador))]
    public class AuditoriaController : NHibernateController
    {
        public virtual ActionResult Index(AuditSearchFiltersViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AuditSearchFiltersViewModel { Desde = DateTime.Now.AddDays(-7).ToMidnigth() };
            return View(viewModel);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual JsonResult List(long count, int page, int rows, AuditSearchFiltersViewModel searchFilters)
        {
            page--;
            var query = GetQuery(searchFilters);
            var respuesta = new PaginableResponse();
            query = query.OrderBy(x => x.Fecha).Desc;

            var queryCount = query.ToRowCountInt64Query().FutureValue<long>();
            respuesta.Records = queryCount.Value;
            respuesta.Rows = query.Take(rows).Skip(page * rows).Future();

            respuesta.Page = ++page;
            respuesta.Total = (long)Math.Ceiling(respuesta.Records / (double)rows);
            return Json(respuesta);
        }

        private IQueryOver<AuditLog, AuditLog> GetQuery(AuditSearchFiltersViewModel searchFilters)
        {
            var session = SessionFactory.GetCurrentSession();
            var query = session.QueryOver<AuditLog>()
                .Where(x => x.Fecha >= searchFilters.Desde)
                .Where(x => x.Fecha <= searchFilters.Hasta);

            if (!string.IsNullOrWhiteSpace(searchFilters.Entidad))
                query = query.And(Restrictions.On<AuditLog>(x => x.Entidad).IsInsensitiveLike(searchFilters.Entidad, MatchMode.Start));
            if (!string.IsNullOrWhiteSpace(searchFilters.Accion))
                query = query.And(x => x.Accion == searchFilters.Accion);
            if (searchFilters.EntidadId.HasValue)
                query = query.And(x => x.EntidadId == searchFilters.EntidadId);
            if (!string.IsNullOrWhiteSpace(searchFilters.Usuario))
                query = query.And(Restrictions.On<AuditLog>(x => x.Usuario).IsInsensitiveLike(searchFilters.Usuario, MatchMode.Start));

            if (!string.IsNullOrWhiteSpace(searchFilters.Filtro))
            {
                query.And(Restrictions.On<AuditLog>(x => x.EntidadAntes).IsInsensitiveLike(searchFilters.Filtro, MatchMode.Start)
                    || Restrictions.On<AuditLog>(x => x.EntidadDespues).IsInsensitiveLike(searchFilters.Filtro, MatchMode.Start));
            }

            return query;
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual JsonResult Get(Guid id)
        {
            var audit = SessionFactory.GetCurrentSession().Get<AuditLog>(id);
            return audit == null ? Json(HttpNotFound()) : Json(audit);
        }

    }
}