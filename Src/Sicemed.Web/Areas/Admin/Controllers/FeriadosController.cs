using System;
using System.Linq;
using System.Linq.Expressions;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
	public class FeriadosController : CrudBaseController<Feriado>
	{
		protected override Expression<Func<Feriado, object>> DefaultOrderBy
		{
			get { return (x) => x.Fecha; }
		}

		protected override System.Collections.IEnumerable AplicarProjections(System.Collections.Generic.IEnumerable<Feriado> results)
		{
			return results.Select(x => new { x.Id, x.Nombre, x.Fecha });
		}
	}
}