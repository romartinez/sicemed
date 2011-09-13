using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class UsuariosController : CrudBaseController<Usuario>
    {
        #region Overrides of CrudBaseController<Usuario>
        protected override Expression<Func<Usuario, object>> DefaultOrderBy
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


        protected override Usuario AgregarReferencias(Usuario modelo)
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