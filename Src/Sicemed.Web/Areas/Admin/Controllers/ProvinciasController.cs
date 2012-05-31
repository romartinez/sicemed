using System;
using System.Linq;
using System.Linq.Expressions;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
	public class ProvinciasController : CrudBaseController<Provincia>
	{
		protected override Expression<Func<Provincia, object>> DefaultOrderBy
		{
			get { return x => x.Nombre; }
		}

		protected override System.Collections.IEnumerable AplicarProjections(System.Collections.Generic.IEnumerable<Provincia> results)
		{
			return results.Select(x => new { x.Id, x.Nombre });
		}
	}
}