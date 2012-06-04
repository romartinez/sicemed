using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels;
using Sicemed.Web.Infrastructure.Helpers;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class PersonasController : NHibernateController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual JsonResult List(long count, int page, int rows)
        {
            page--;
            var session = SessionFactory.GetCurrentSession();
            var query = session.QueryOver<Persona>();

            var respuesta = new PaginableResponse();
            query.OrderBy(x => x.Membership.Email);

            if (page == 0)
            {
                var queryCount = query.ToRowCountInt64Query().FutureValue<long>();
                respuesta.Records = queryCount.Value;
            }
            else
            {
                respuesta.Records = count;
            }
            respuesta.Rows = AplicarProjections(query.Take(rows).Skip(page * rows).Future());

            respuesta.Page = ++page;
            respuesta.Total = (long)Math.Ceiling(respuesta.Records / (double)rows);
            return Json(respuesta);
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

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public void EliminarUsuario(long usuarioId)
        {
            var session = SessionFactory.GetCurrentSession();
            var user = session.Get<Persona>(usuarioId);
            if (user == null) throw new ValidationErrorException("El usuario no existe.");

            session.Delete<Persona>(usuarioId);
            
            ShowMessages(ResponseMessage.Success("Usuario eliminado."));
        }

        protected IEnumerable AplicarProjections(IEnumerable<Persona> results)
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