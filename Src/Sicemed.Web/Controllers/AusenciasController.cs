using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Criterion;
using NHibernate.Transform;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Queries.ObtenerTurno;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Secretaria))]
    [AuthorizeIt(typeof(Administrador))]
    public class AusenciasController : NHibernateController
    {
        public ActionResult Index()
        {
            var profesionales = SessionFactory.GetCurrentSession().QueryOver<Profesional>().JoinQueryOver(x => x.Persona).OrderBy(x => x.Apellido).Asc.Future();
            var listado = profesionales.Select(p => new { p.Id, Text = p.Persona.NombreCompleto });
            return View(listado);
        }

        protected System.Collections.IEnumerable AplicarProjections(System.Collections.Generic.IEnumerable<Ausencia> results)
        {
            return results.Select(x => new
            {
                x.Id,
                x.Fecha,
                x.Desde,
                x.Hasta,
                Profesional = x.Profesional != null ? new { x.Profesional.Id, x.Profesional.Persona.NombreCompleto } : null
            });
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual JsonResult List(long count, int page, int rows, string sidx, string sord, string searchField, string searchString, string searchOper)
        {
            page--;
            var session = SessionFactory.GetCurrentSession();
            var query = session.QueryOver<Ausencia>();
            Persona aliasPersona = null;
            query.JoinQueryOver(c => c.Profesional).JoinQueryOver(c => c.Persona, () => aliasPersona);

            if (!string.IsNullOrEmpty(searchField))
            {
                switch (searchField.ToLower())
                {
                    case "profesional":
                        query.Where(Restrictions.Or(
                            Restrictions.On<Persona>(x => aliasPersona.Apellido).IsInsensitiveLike(searchString, MatchMode.Anywhere), 
                            Restrictions.On<Persona>(x => aliasPersona.Nombre).IsInsensitiveLike(searchString, MatchMode.Anywhere)
                        ));
                        break;
                    case "fecha":
                        DateTime fecha;
                        try
                        {
                            fecha = DateTime.Parse(searchString).ToMidnigth();                            
                        }catch(Exception ex)
                        {
                            throw new ValidationException(string.Format("No se reconoce el valor '{0}' como una fecha válida.", searchString));
                        }
                        query.Where(x => x.Fecha == fecha);
                        break;
                }
            }

            if (string.IsNullOrEmpty(sidx))
            {
                query = query.OrderBy(x => x.Fecha).Desc;
            }
            else
            {
                switch (sidx.ToLower())
                {
                    case "fecha":
                        query = (sord == "asc") ? query.OrderBy(x => x.Fecha).Asc : query.OrderBy(x => x.Fecha).Desc;
                        break;
                    case "profesional":
                        query = (sord == "asc") ? query.OrderBy(x => aliasPersona.Apellido).Asc : query.OrderBy(x => aliasPersona.Apellido).Desc;
                        break;
                }
            }

            query = query.TransformUsing(Transformers.DistinctRootEntity);

            var respuesta = new PaginableResponse();

            if (page == 0)
            {
                var queryCount = query.ToRowCountInt64Query().FutureValue<long>();
                respuesta.Records = queryCount.Value;
            }
            else
            {
                respuesta.Records = count;
            }
            var entites = query.Skip(page * rows).Take(rows).Future();
            respuesta.Rows = AplicarProjections(entites);

            respuesta.Page = ++page;
            respuesta.Total = (long)Math.Ceiling(respuesta.Records / (double)rows);
            return Json(respuesta);
        }


        protected Ausencia AgregarReferencias(Ausencia modelo)
        {
            modelo.Profesional = ObtenerProfesionalSeleccionado();

            //Limpio Cache de turnos
            var query = QueryFactory.Create<IObtenerTurnosProfesionalQuery>();
            query.ProfesionalId = modelo.Profesional.Id;
            query.ClearCache();

            return modelo;
        }

        private Profesional ObtenerProfesionalSeleccionado()
        {
            const string ERROR_PROFESIONAL_NO_ENCONTRADO = @"Debe seleccionar un Profesional para la Ausencia.";

            var profesionalId = RetrieveParameter<long>("profesionalId", "Profesional");
            var session = SessionFactory.GetCurrentSession();

            var profesional = session.Get<Profesional>(profesionalId);

            if (profesional == null)
                throw new ValidationErrorException(ERROR_PROFESIONAL_NO_ENCONTRADO);

            return profesional;
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute]
        public virtual void Nuevo(string oper, Ausencia modelo, int paginaId = 0)
        {
            if (!oper.Equals("add", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            AgregarReferencias(modelo);

            SessionFactory.GetCurrentSession().Save(modelo);

            ShowMessages(ResponseMessage.Success());
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute]
        public virtual void Editar(long id, string oper, Ausencia modelo)
        {
            if (!oper.Equals("edit", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            var session = SessionFactory.GetCurrentSession();

            var modelFromDb = session.QueryOver<Ausencia>().Where(x => x.Id == id).SingleOrDefault();

            UpdateModel(modelFromDb);

            AgregarReferencias(modelFromDb);

            ShowMessages(ResponseMessage.Success());
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute]
        public virtual void Eliminar(string id, string oper)
        {
            if (!oper.Equals("del", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            var idsSeleccionados = id.Split(',');
            var session = SessionFactory.GetCurrentSession();
            foreach (var idsSeleccionado in idsSeleccionados)
            {
                session.Delete<Ausencia>(Convert.ToInt64(idsSeleccionado));
            }

            ShowMessages(ResponseMessage.Success());
        }

    }
}