using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using NHibernate;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models.BI;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class ObjetivosController : CrudBaseController<ObjetivoIndicador>
	{
        protected override Expression<Func<ObjetivoIndicador, object>> DefaultOrderBy
		{
			get { return x => x.Indicador.Nombre; }
		}

		public override ActionResult Index()
		{
			return View(SessionFactory.GetCurrentSession().QueryOver<Indicador>().OrderBy(x => x.Nombre).Asc.Future());
		}

        protected override System.Collections.IEnumerable AplicarProjections(System.Collections.Generic.IEnumerable<ObjetivoIndicador> results)
		{
			return results.Select(x => new
			{
				x.Id,				
				x.Anio,
				x.Mes,
                x.ValorMaximo,
                x.ValorMinimo,
                Indicador = x.Indicador != null ? new { x.Indicador.Id, x.Indicador.Nombre } : null
			});
		}

        protected override IQueryOver<ObjetivoIndicador> AplicarFetching(IQueryOver<ObjetivoIndicador, ObjetivoIndicador> query)
		{
			return query.Fetch(x => x.Indicador).Eager;
		}

        protected override ObjetivoIndicador AgregarReferencias(ObjetivoIndicador modelo)
		{
			modelo.Indicador = ObtenerIndicadorSeleccionado();

			return modelo;
		}

		private Indicador ObtenerIndicadorSeleccionado()
		{
			const string ERROR_INDICADOR_NO_ENCONTRADO = @"Debe seleccionar un Indicador para el Objetivo.";

			var indicadorId = RetrieveParameter<long>("indicadorId", "Indicador");
			var session = SessionFactory.GetCurrentSession();

            var indicador = session.Get<Indicador>(indicadorId);

			if (indicador == null)
				throw new ValidationErrorException(ERROR_INDICADOR_NO_ENCONTRADO);

			return indicador;
		}
	}
}