using System;
using System.Linq;
using System.Linq.Expressions;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models.BI;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class CategoriasIndicadorController : CrudBaseController<CategoriaIndicador>
	{
        protected override Expression<Func<CategoriaIndicador, object>> DefaultOrderBy
		{
			get { return x => x.Nombre; }
		}

        protected override System.Collections.IEnumerable AplicarProjections(System.Collections.Generic.IEnumerable<CategoriaIndicador> results)
		{
			return results.Select(x => new
			{
				x.Id,
				x.Nombre,
			});
		}
	}
}