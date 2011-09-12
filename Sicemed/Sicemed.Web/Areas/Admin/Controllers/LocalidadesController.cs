using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using NHibernate;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class LocalidadesController : CrudBaseController<Localidad>
    {
        #region Overrides of CrudBaseController<Localidad>

        protected override Expression<Func<Localidad, object>> DefaultOrderBy
        {
            get { return x => x.Nombre; }
        }

        public override ActionResult Index()
        {
            return View(SessionFactory.GetCurrentSession().QueryOver<Provincia>().OrderBy(x => x.Nombre).Asc.Future());
        }

        protected override System.Collections.IEnumerable RetrieveList(int page, int rows, NHibernate.IQueryOver<Localidad, Localidad> query)
        {
            return query.JoinQueryOver<Provincia>(x => x.Provincia).Take(rows).Skip(page*rows).Future();
        }

        public override JsonResult Nuevo(string oper, Localidad modelo, int paginaId = 0)
        {
            if (!oper.Equals("add", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            var session = SessionFactory.GetCurrentSession();

            modelo.Provincia = RetrieveProvincia();

            session.Save(modelo);

            return Json(ResponseMessage.Success());
        }

        public override ActionResult Editar(long id, string oper, Localidad modelo)
        {
            if (!oper.Equals("edit", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            var session = SessionFactory.GetCurrentSession();

            var modelFromDb = session.QueryOver<Localidad>().Where(x => x.Id == id).SingleOrDefault();

            UpdateModel(modelFromDb);

            modelFromDb.Provincia = RetrieveProvincia();

            return Json(ResponseMessage.Success());      
        }


        private Provincia RetrieveProvincia()
        {
            const string ERROR_PROVINCIA_NO_ENCONTRADA = @"Debe seleccionar una Provincia para la Localidad.";

            var provinciaIdValue = this.ValueProvider.GetValue("provinciaId");
            if (provinciaIdValue == null)
                throw new ValidationErrorException(ERROR_PROVINCIA_NO_ENCONTRADA);
            if (string.IsNullOrWhiteSpace(provinciaIdValue.AttemptedValue))
                throw new ValidationErrorException(ERROR_PROVINCIA_NO_ENCONTRADA);
            long provinciaId = 0;
            long.TryParse(provinciaIdValue.AttemptedValue, out provinciaId);
            if (provinciaId == 0) throw new ValidationErrorException(ERROR_PROVINCIA_NO_ENCONTRADA);

            var session = SessionFactory.GetCurrentSession();

            var provincia = session.Get<Provincia>(provinciaId);

            if (provincia == null)
                throw new ValidationErrorException(ERROR_PROVINCIA_NO_ENCONTRADA);

            return provincia;
        }

        #endregion
    }
}