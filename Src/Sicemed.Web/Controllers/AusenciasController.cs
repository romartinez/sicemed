using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using NHibernate;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Infrastructure.Queries.ObtenerTurno;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Secretaria))]
    [AuthorizeIt(typeof(Administrador))]
    public class AusenciasController : CrudBaseController<Ausencia>
    {
        protected override Expression<Func<Ausencia, object>> DefaultOrderBy
        {
            get { return x => x.Fecha; }
        }

        public override ActionResult Index()
        {
            return View(SessionFactory.GetCurrentSession().QueryOver<Profesional>().OrderBy(x => x.Id).Asc.Future());
        }

        protected override System.Collections.IEnumerable AplicarProjections(System.Collections.Generic.IEnumerable<Ausencia> results)
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

        protected override IQueryOver<Ausencia> AplicarFetching(IQueryOver<Ausencia, Ausencia> query)
        {
            var q = query;
            q.JoinQueryOver(x => x.Profesional).JoinQueryOver(x => x.Persona);
            return q;
        }

        protected override Ausencia AgregarReferencias(Ausencia modelo)
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
    }
}