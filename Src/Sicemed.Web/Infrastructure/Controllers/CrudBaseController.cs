using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using NHibernate;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Infrastructure.Controllers
{
    [AuthorizeIt(typeof (Administrador))]
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

            var respuesta = new PaginableResponse();
            query.OrderBy(DefaultOrderBy);

            if (page == 0)
            {
                var queryCount = query.ToRowCountInt64Query().FutureValue<long>();
                respuesta.Records = queryCount.Value;
            } else
            {
                respuesta.Records = count;
            }
            respuesta.Rows = AplicarProjections(AplicarFetching(query).Take(rows).Skip(page*rows).Future());

            respuesta.Page = ++page;
            respuesta.Total = (long) Math.Ceiling(respuesta.Records/(double) rows);
            return Json(respuesta);
        }

        protected virtual IEnumerable AplicarProjections(IEnumerable<T> results)
        {
            return results;
        }

        protected virtual IQueryOver<T> AplicarFetching(IQueryOver<T, T> query)
        {
            return query;
        }

        protected virtual bool EsValido(T modelo)
        {
            return true;
        }

        protected virtual T AgregarReferencias(T modelo)
        {
            return modelo;
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute]
        public virtual JsonResult Nuevo(string oper, T modelo, int paginaId = 0)
        {
            if (!oper.Equals("add", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            AgregarReferencias(modelo);

            EsValido(modelo);

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

            AgregarReferencias(modelFromDb);

            EsValido(modelFromDb);

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

        public virtual ActionResult ObtenerLocalidadesPorProvincia(long provinciaId)
        {
            using (var session = SessionFactory.OpenStatelessSession())
            {
                ViewData.Model = session.QueryOver<Localidad>()
                    .Fetch(x => x.Provincia).Eager
                    .Where(x => x.Provincia.Id == provinciaId).List();
                return PartialView();
            }
        }
    }
}