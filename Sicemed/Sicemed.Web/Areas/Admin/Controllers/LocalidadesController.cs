using System;
using System.Linq.Expressions;
using System.Web.Mvc;
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

        public override JsonResult Nuevo(string oper, Localidad modelo, int paginaId = 0)
        {
            if (!oper.Equals("add", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();
            var provinciaIdValue = this.ValueProvider.GetValue("provinciaId");
            if(provinciaIdValue == null) throw new ValidationErrorException("Debe seleccionar una Provincia para la Localidad.");
            if(string.IsNullOrWhiteSpace(provinciaIdValue.AttemptedValue)) throw new ValidationErrorException("Debe seleccionar una Provincia para la Localidad.");
            long provinciaId = 0;
            long.TryParse(provinciaIdValue.AttemptedValue, out provinciaId);
            if(provinciaId == 0) throw new ValidationErrorException("Debe seleccionar una Provincia para la Localidad.");

            var session = SessionFactory.GetCurrentSession();

            var provincia = session.Get<Provincia>(provinciaId);

            if (provincia == null)
                throw new ValidationErrorException("Debe seleccionar una Provincia para la Localidad.");            

            provincia.AgregarLocalidad(modelo);

            return Json(ResponseMessage.Success());
        }
        #endregion
    }
}