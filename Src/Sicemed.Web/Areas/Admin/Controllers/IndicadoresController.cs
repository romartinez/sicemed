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
    public class IndicadoresController : CrudBaseController<Indicador>
	{
        protected override Expression<Func<Indicador, object>> DefaultOrderBy
		{
			get { return x => x.Nombre; }
		}

		public override ActionResult Index()
		{
			return View(SessionFactory.GetCurrentSession().QueryOver<CategoriaIndicador>().OrderBy(x => x.Nombre).Asc.Future());
		}

        protected override System.Collections.IEnumerable AplicarProjections(System.Collections.Generic.IEnumerable<Indicador> results)
		{
			return results.Select(x => new
			{
				x.Id,
				x.Nombre,
				x.Descripcion,
				x.Habilitado,
                x.TipoOperador,
                x.NumeradorSql,
                x.DenominadorSql,
				Categoria = x.Categoria != null ? new { x.Categoria.Id, x.Categoria.Nombre } : null
			});
		}

        protected override IQueryOver<Indicador> AplicarFetching(IQueryOver<Indicador, Indicador> query)
		{
			return query.Fetch(x => x.Categoria).Eager;
		}

        protected override Indicador AgregarReferencias(Indicador modelo)
		{
			modelo.Categoria = ObtenerCategoriaSeleccionada();

			return modelo;
		}

		private CategoriaIndicador ObtenerCategoriaSeleccionada()
		{
			const string ERROR_CATEGORIA_NO_ENCONTRADA = @"Debe seleccionar una Categoria para el Indicador.";

			var provinciaId = RetrieveParameter<long>("categoriaId", "Categoria");
			var session = SessionFactory.GetCurrentSession();

			var categoriaIndicador = session.Get<CategoriaIndicador>(provinciaId);

			if (categoriaIndicador == null)
				throw new ValidationErrorException(ERROR_CATEGORIA_NO_ENCONTRADA);

			return categoriaIndicador;
		}
	}
}