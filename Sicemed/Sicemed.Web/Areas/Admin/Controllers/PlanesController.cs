using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using NHibernate;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class PlanesController : CrudBaseController<Plan>
    {
        #region Overrides of CrudBaseController<Plan>

        protected override Expression<Func<Plan, object>> DefaultOrderBy
        {
            get { return x => x.Nombre; }
        }

        public override ActionResult Index()
        {
            return View(SessionFactory.GetCurrentSession().QueryOver<ObraSocial>().OrderBy(x => x.RazonSocial).Asc.Future());
        }

        protected override IQueryOver<Plan> AplicarFetching(IQueryOver<Plan, Plan> query)
        {
            return query.Fetch(x => x.ObraSocial).Eager;
        }

        protected override Plan AgregarReferencias(Plan modelo)
        {
            modelo.ObraSocial = ObtenerObraSocialSeleccionada();

            return modelo;
        }

        protected override System.Collections.IEnumerable AplicarProjections(System.Collections.Generic.IEnumerable<Plan> results)
        {
            return results.Select(x=> new
                                      {
                                          x.Id,
                                          x.Nombre,
                                          x.Descripcion,
                                          ObraSocial = x.ObraSocial != null ? new
                                                                              {
                                                                                  x.ObraSocial.Id,
                                                                                  x.ObraSocial.RazonSocial
                                                                              } : null
                                      });
        }

        private ObraSocial ObtenerObraSocialSeleccionada()
        {
            const string ERROR_OBRA_SOCIAL_NO_SELECCIONADA = @"Debe seleccionar una Obra Social para el Plan.";

            var obraSocialId = RetrieveParameter<long>("obraSocialId", "Obra Social");
            var session = SessionFactory.GetCurrentSession();

            var obraSocial = session.Get<ObraSocial>(obraSocialId);

            if (obraSocial == null)
                throw new ValidationErrorException(ERROR_OBRA_SOCIAL_NO_SELECCIONADA);

            return obraSocial;
        }

        #endregion
    }
}