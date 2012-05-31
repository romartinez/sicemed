using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
	public class ConsultoriosController : CrudBaseController<Consultorio>
	{
		protected override Expression<Func<Consultorio, object>> DefaultOrderBy
		{
			get { return x => x.Nombre; }
		}

		protected override System.Collections.IEnumerable AplicarProjections(IEnumerable<Consultorio> results)
		{
			return results.Select(x => new { x.Id, x.Nombre, x.Descripcion });
		}
	}
}