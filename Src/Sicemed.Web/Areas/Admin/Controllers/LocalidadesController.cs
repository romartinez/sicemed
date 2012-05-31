using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using NHibernate;
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

        protected override IQueryOver<Localidad> AplicarFetching(IQueryOver<Localidad, Localidad> query)
        {
            return query.Fetch(x => x.Provincia).Eager;
        }

        protected override Localidad AgregarReferencias(Localidad modelo)
        {
            modelo.Provincia = ObtenerProvinciaSeleccionada();

            return modelo;
        }

        private Provincia ObtenerProvinciaSeleccionada()
        {
            const string ERROR_PROVINCIA_NO_ENCONTRADA = @"Debe seleccionar una Provincia para la Localidad.";

            var provinciaId = RetrieveParameter<long>("provinciaId", "Provincia");
            var session = SessionFactory.GetCurrentSession();

            var provincia = session.Get<Provincia>(provinciaId);

            if (provincia == null)
                throw new ValidationErrorException(ERROR_PROVINCIA_NO_ENCONTRADA);

            return provincia;
        }

        #endregion
    }
}