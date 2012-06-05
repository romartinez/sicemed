using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class PersonasController : CrudBaseController<Persona>
    {
        protected override Expression<Func<Persona, object>> DefaultOrderBy
        {
            get { return x => x.Membership.Email; }
        }

        public override ActionResult Editar(long id, string oper, Persona modelo)
        {
            throw new NotSupportedException("Use la ventana de editar.");
        }

        public override JsonResult Nuevo(string oper, Persona modelo, int paginaId = 0)
        {
            throw new NotSupportedException("Use la ventana de nuevo.");
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public void BloquearUsuario(long usuarioId)
        {
            var session = SessionFactory.GetCurrentSession();
            var user = session.Get<Persona>(usuarioId);
            if (user == null) throw new ValidationErrorException("El usuario no existe.");
            if (user.Membership.IsLockedOut)
                throw new ValidationErrorException("El usuario ya se encuentra bloqueado.");

            MembershipService.LockUser(user.Membership.Email,
                                       string.Format("{0:dd/mm/yy} - {1} - Bloqueo Administrativo", DateTime.Now, User));

            ShowMessages(ResponseMessage.Success("Bloqueo realizado con éxito."));
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public void DesbloquearUsuario(long usuarioId)
        {
            var session = SessionFactory.GetCurrentSession();
            var user = session.Get<Persona>(usuarioId);
            if (user == null) throw new ValidationErrorException("El usuario no existe.");
            if (!user.Membership.IsLockedOut)
                throw new ValidationErrorException("El usuario ya se encuentra desbloqueado.");

            MembershipService.UnlockUser(user.Membership.Email);
            
            ShowMessages(ResponseMessage.Success("Desbloqueo realizado con éxito."));
        }

        protected override IEnumerable AplicarProjections(IEnumerable<Persona> results)
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
                                           x.NombreCompleto,
                                           x.FechaNacimiento,
                                           x.Telefono,
                                           Roles = x.Roles.Select(r => r.DisplayName),
                                           Membership = x.Membership != null
                                                            ? new
                                                              {
                                                                  x.Membership.Email,
                                                                  x.Membership.IsLockedOut
                                                              }
                                                            : null
                                       });
        }
    }
}