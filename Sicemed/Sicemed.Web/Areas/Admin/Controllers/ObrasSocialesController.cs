using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class ObrasSocialesController : CrudBaseController<ObraSocial>
    {
        #region Overrides of CrudBaseController<ObraSocial>

        protected override Expression<Func<ObraSocial, object>> DefaultOrderBy
        {
            get { return x => x.RazonSocial; }
        }

        protected override NHibernate.IQueryOver<ObraSocial> AplicarFetching(NHibernate.IQueryOver<ObraSocial, ObraSocial> query)
        {
            return query.Fetch(x => x.Domicilio.Localidad).Eager.Fetch(x => x.Domicilio.Localidad.Provincia).Eager;
        }

        protected override System.Collections.IEnumerable AplicarProjections(System.Collections.Generic.IEnumerable<ObraSocial> results)
        {
            return results.Select(x => new
                                  {
                                      x.Documento,
                                      Domicilio = new
                                                  {
                                                      x.Domicilio.Direccion,
                                                      Localidad = new
                                                                  {
                                                                      x.Domicilio.Localidad.Id,
                                                                      x.Domicilio.Localidad.Nombre,
                                                                      Provincia = new
                                                                                  {
                                                                                      x.Domicilio.Localidad.Provincia.Id,
                                                                                      x.Domicilio.Localidad.Provincia.Nombre
                                                                                  }
                                                                  }
                                                  },
                                      x.Id,
                                      x.RazonSocial,
                                      x.Telefono
                                  });
        }

        public override ActionResult Index()
        {
            using(var session = SessionFactory.OpenStatelessSession())
            {
                ViewData.Model = session.QueryOver<Provincia>().List();
            }            
            return base.Index();
        }

        protected override ObraSocial AgregarReferencias(ObraSocial modelo)
        {
            var localidadId = RetrieveParameter<long>("localidadId", "Localidad", true);

            var session = SessionFactory.GetCurrentSession();

            Localidad localidad = null;
            if(localidadId > 0)
            {
                localidad = session.Get<Localidad>(localidadId);
                if (localidad == null) throw new ValidationErrorException("La Localidad seleccionada no existe.");
            }

            modelo.Domicilio.Localidad = localidad;

            return base.AgregarReferencias(modelo);
        }
        #endregion

        public ActionResult ObtenerLocalidadesPorProvincia(long provinciaId)
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