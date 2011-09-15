using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class PersonasController : CrudBaseController<Persona>
    {
        #region Overrides of CrudBaseController<Persona>
        protected override Expression<Func<Persona, object>> DefaultOrderBy
        {
            get { return x => x.Membership.Email; }
        }

        public override ActionResult Index()
        {
            using (var session = SessionFactory.OpenStatelessSession())
            {
                ViewData.Model = session.QueryOver<Provincia>().List();
            }
            return base.Index();
        }

        protected override System.Collections.IEnumerable AplicarProjections(System.Collections.Generic.IEnumerable<Persona> results)
        {
            return results.Select(x => new
            {
                x.Documento,
                Domicilio = x.Domicilio != null ? new
                {
                    x.Domicilio.Direccion,
                    Localidad = x.Domicilio.Localidad != null ? new
                    {
                        x.Domicilio.Localidad.Id,
                        x.Domicilio.Localidad.Nombre,
                        Provincia = x.Domicilio.Localidad.Provincia != null ? new
                        {
                            x.Domicilio.Localidad.Provincia.Id,
                            x.Domicilio.Localidad.Provincia.Nombre
                        } : null
                    } : null
                } : null,
                x.Id,
                x.Apellido,
                x.Nombre,
                x.SegundoNombre,
                x.FechaNacimiento,
                x.Telefono,
                Roles = x.Roles.Select(r=> r.DisplayName),
                Membership = x.Membership != null ?  new
                                 {
                                     x.Membership.Email,
                                     x.Membership.IsLockedOut
                                 } : null                
            });
        }

        protected override Persona AgregarReferencias(Persona modelo)
        {
            var tipoDocumentoId = RetrieveParameter<int>("Documento.TipoDocumento.Value", "Tipo De Documento");
            var tipoDocumento = Enumeration.FromValue<TipoDocumento>(tipoDocumentoId);
            if (tipoDocumento == null) throw new ValidationErrorException("Debe seleccionar un Tipo De Documento válido.");

            var password = RetrieveParameter<string>("Password");
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
            modelo.Membership.Password = password;

            return base.AgregarReferencias(modelo);
        }

        #endregion

    }
}