using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using NHibernate;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class ObrasSocialesController : CrudBaseController<ObraSocial>
    {
        protected override Expression<Func<ObraSocial, object>> DefaultOrderBy
        {
            get { return x => x.RazonSocial; }
        }

        protected override IQueryOver<ObraSocial> AplicarFetching(IQueryOver<ObraSocial, ObraSocial> query)
        {
            return query.Fetch(x => x.Domicilio.Localidad).Eager.Fetch(x => x.Domicilio.Localidad.Provincia).Eager;
        }

        protected override IEnumerable AplicarProjections(IEnumerable<ObraSocial> results)
        {
            return results.Select(x => new
                                       {
                                           x.Documento,
                                           Domicilio = x.Domicilio != null
                                                           ? new
                                                             {
                                                                 x.Domicilio.Direccion,
                                                                 Localidad = x.Domicilio.Localidad != null
                                                                                 ? new
                                                                                   {
                                                                                       x.Domicilio.Localidad.Id,
                                                                                       x.Domicilio.Localidad.Nombre,
                                                                                       Provincia =
                                                                                       x.Domicilio.Localidad.Provincia !=
                                                                                       null
                                                                                           ? new
                                                                                             {
                                                                                                 x.Domicilio.Localidad.
                                                                                                 Provincia.Id,
                                                                                                 x.Domicilio.Localidad.
                                                                                                 Provincia.Nombre
                                                                                             }
                                                                                           : null
                                                                                   }
                                                                                 : null
                                                             }
                                                           : null,
                                           x.Id,
                                           x.RazonSocial,
                                           x.Telefono
                                       });
        }

        public override ActionResult Index()
        {
            using (var session = SessionFactory.OpenStatelessSession())
            {
                ViewData.Model = session.QueryOver<Provincia>().List();
            }
            return base.Index();
        }

        protected override ObraSocial AgregarReferencias(ObraSocial modelo)
        {
            var tipoDocumentoId = RetrieveParameter<int>("Documento.TipoDocumento.Value", "Tipo De Documento");
            var tipoDocumento = Enumeration.FromValue<TipoDocumento>(tipoDocumentoId);
            if (tipoDocumento == null)
                throw new ValidationErrorException("Debe seleccionar un Tipo De Documento válido.");

            var localidadId = RetrieveParameter<long>("localidadId", "Localidad", true);

            var session = SessionFactory.GetCurrentSession();

            Localidad localidad = null;
            if (localidadId > 0)
            {
                localidad = session.Get<Localidad>(localidadId);
                if (localidad == null) throw new ValidationErrorException("La Localidad seleccionada no existe.");
            }

            modelo.Domicilio.Localidad = localidad;
            modelo.Documento.TipoDocumento = tipoDocumento;


            return base.AgregarReferencias(modelo);
        }
    }
}