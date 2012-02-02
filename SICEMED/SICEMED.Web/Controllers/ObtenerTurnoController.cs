using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Criterion;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt]
    public class ObtenerTurnoController : NHibernateController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual JsonResult ObtenerEspecialidades()
        {
            var session = SessionFactory.GetCurrentSession();
            var especialidades = session.QueryOver<Especialidad>()
                .OrderBy(x=>x.Nombre).Asc
                .Future()
                .Select(x => new
                {
                    x.Id,
                    x.Nombre
                });
            return Json(especialidades, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult BuscarProfesional(long? especialidadId, string nombre)
        {
            var session = SessionFactory.GetCurrentSession();
            var query = session.QueryOver<Persona>()
                .WhereRestrictionOn(p => p.Nombre).IsLike(nombre, MatchMode.Start)
                .WhereRestrictionOn(p => p.SegundoNombre).IsLike(nombre, MatchMode.Start)
                .WhereRestrictionOn(p => p.Apellido).IsLike(nombre, MatchMode.Start)
                .JoinQueryOver<Rol>(p => p.Roles)
                .Where(r=>r is Profesional);

            if(especialidadId.HasValue)
            {
                query = query.Where(r=>r.Id == especialidadId.Value);
            }

            var profesionales = query.Future().Select(p => new
                                                               {
                                                                   p.Id,
                                                                   Foto = "/public/images/personal_128x128.png",
                                                                   Nombre = string.Format("{0}, {1} {2}", p.Apellido, p.Nombre, p.SegundoNombre),
                                                                   ProximoTurno = DateTime.Now,
                                                                   Especialidades = p.As<Profesional>().Especialidades.Select(e=> e.Nombre)
                                                               });

            return Json(profesionales, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult ObtenerAgendaProfesional(long profesionalId)
        {
            throw new NotImplementedException();
        }

        public virtual JsonResult ReservarTurno(long profesionalId, DateTime fecha)
        {
            throw new NotImplementedException();
        }

        public virtual ActionResult ImprimirComprobante(long turnoId)
        {
            throw new NotImplementedException();
        }
    }
}