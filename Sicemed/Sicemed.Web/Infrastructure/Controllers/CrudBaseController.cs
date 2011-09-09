using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations.Roles;
using Sicemed.Web.Models.ViewModels;
using Sicemed.Web.Infrastructure.Helpers;

namespace Sicemed.Web.Infrastructure.Controllers
{
    [AuthorizeIt(typeof(Administrador))]
    public abstract class CrudBaseController<T> : NHibernateController where T : Entity
    {
        protected abstract Expression<Func<T, object>> DefaultOrderBy { get; }

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
            var query = session.QueryOver<T>();

            var respuesta = new PaginableResponse<T>();
            query.OrderBy(DefaultOrderBy);
            respuesta.Rows = query.Take(rows).Skip(page * rows).Future<T>();
            if (page == 0)
            {
                var queryCount = query.ToRowCountInt64Query().FutureValue<long>();
                respuesta.Records = queryCount.Value;
            }
            else
            {
                respuesta.Records = count;
            }
            respuesta.Page = ++page;
            respuesta.Total = (long)Math.Ceiling(respuesta.Records / (double)rows);
            return Json(respuesta);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute]
        public virtual JsonResult Nuevo(string oper, T modelo, int paginaId = 0)
        {
            if (!oper.Equals("add", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            SessionFactory.GetCurrentSession().Save(modelo);

            return Json(ResponseMessage.Success());
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute]
        public virtual ActionResult Editar(long id, string oper, T modelo)
        {
            if (!oper.Equals("edit", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            var session = SessionFactory.GetCurrentSession();

            var modelFromDb = session.QueryOver<T>().Where(x => x.Id == id).SingleOrDefault();

            UpdateModel(modelFromDb);

            return Json(ResponseMessage.Success());
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute]
        public virtual ActionResult Eliminar(string id, string oper)
        {
            if (!oper.Equals("del", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            var idsSeleccionados = id.Split(',');
            var session = SessionFactory.GetCurrentSession();
            foreach (var idsSeleccionado in idsSeleccionados)
            {
                session.Delete<T>(Convert.ToInt64(idsSeleccionado));
            }

            return Json(ResponseMessage.Success());
        }
    }
}