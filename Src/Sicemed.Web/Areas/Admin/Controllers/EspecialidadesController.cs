using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
	public class EspecialidadesController : CrudBaseController<Especialidad>
	{
		protected override Expression<Func<Especialidad, object>> DefaultOrderBy
		{
			get { return x => x.Nombre; }
		}

		protected override IEnumerable AplicarProjections(IEnumerable<Especialidad> results)
		{
			return results.Select(x => new { x.Id, x.Nombre, x.Descripcion });
		}
	}
}