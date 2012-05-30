using System;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    [AuthorizeIt(typeof(Administrador))]
    public class AuditoriaController : NHibernateController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual JsonResult List(long count, int page, int rows)
        {
            page--;
            var session = SessionFactory.GetCurrentSession();
            var query = session.QueryOver<AuditLog>();

            var respuesta = new PaginableResponse();
            query = query.OrderBy(x=>x.Fecha).Desc;

            if (page == 0)
            {
                var queryCount = query.ToRowCountInt64Query().FutureValue<long>();
                respuesta.Records = queryCount.Value;
            }
            else
            {
                respuesta.Records = count;
            }
            respuesta.Rows = query.Take(rows).Skip(page * rows).Future();

            respuesta.Page = ++page;
            respuesta.Total = (long)Math.Ceiling(respuesta.Records / (double)rows);
            return Json(respuesta);
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