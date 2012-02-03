using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
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
                .OrderBy(x => x.Nombre).Asc
                .Future()
                .Select(x => new
                {
                    x.Id,
                    x.Nombre
                });
            return Json(especialidades, JsonRequestBehavior.AllowGet);
        }

        #region BuscarProfesional

        public virtual JsonResult BuscarProfesional(long? especialidadId, string nombre)
        {
            return especialidadId.HasValue ?
                BuscarProfesionalPorEspecialidad(especialidadId.Value, nombre)
                : BuscarProfesionalPorNombre(nombre);
        }

        private JsonResult BuscarProfesionalPorNombre(string nombre)
        {
            var session = SessionFactory.GetCurrentSession();

            var query = session.QueryOver<Persona>()
                    .Where(
                        Restrictions.On<Persona>(p => p.Nombre).IsLike(nombre, MatchMode.Start)
                        || Restrictions.On<Persona>(p => p.SegundoNombre).IsLike(nombre, MatchMode.Start)
                        || Restrictions.On<Persona>(p => p.Apellido).IsLike(nombre, MatchMode.Start)
                    ).JoinQueryOver<Rol>(p => p.Roles)
                    .Where(r => r.GetType() == typeof(Profesional))
                    .TransformUsing(Transformers.DistinctRootEntity)
                    .Future();

            var profesionales = query.Select(ConverPersonaToProfesionalViewModel);

            return Json(profesionales, JsonRequestBehavior.AllowGet);
        }

        private JsonResult BuscarProfesionalPorEspecialidad(long especialidadId, string nombre)
        {
            var session = SessionFactory.GetCurrentSession();

            var especialidadConProfesionales = session.QueryOver<Especialidad>()
                .Where(e => e.Id == especialidadId)
                .JoinQueryOver<Profesional>(e => e.Profesionales)
                .JoinQueryOver(p => p.Persona)
                .Where(
                    Restrictions.On<Persona>(p => p.Nombre).IsLike(nombre, MatchMode.Start)
                    || Restrictions.On<Persona>(p => p.SegundoNombre).IsLike(nombre, MatchMode.Start)
                    || Restrictions.On<Persona>(p => p.Apellido).IsLike(nombre, MatchMode.Start)
                )
                .TransformUsing(Transformers.DistinctRootEntity)
                .Future();

            var profesionalesConEspecialidad = especialidadConProfesionales
                .SelectMany(e => e.Profesionales)
                .Select(p => p.Persona)
                .Select(ConverPersonaToProfesionalViewModel);

            return Json(profesionalesConEspecialidad, JsonRequestBehavior.AllowGet);
        }

        private static dynamic ConverPersonaToProfesionalViewModel(Persona p)
        {
            return new
            {
                p.Id,
                Foto = "/public/images/personal_128x128.png",
                Nombre = string.Format("{0}, {1} {2}", p.Apellido, p.Nombre, p.SegundoNombre),
                ProximoTurno = DateTime.Now,
                Especialidades = p.As<Profesional>().Especialidades.Select(e => e.Nombre)
            };
        }

        #endregion

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