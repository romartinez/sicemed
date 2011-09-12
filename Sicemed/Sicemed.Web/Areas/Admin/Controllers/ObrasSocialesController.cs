using System;
using System.Linq;
using System.Linq.Expressions;
using Sicemed.Web.Infrastructure.Controllers;
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
            return query.Fetch(x => x.Domicilio.Localidad).Eager;
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
                                                                      x.Domicilio.Localidad.Nombre
                                                                  }
                                                  },
                                      x.Id,
                                      x.RazonSocial,
                                      x.Telefono
                                  });
        }

        #endregion
    }
}