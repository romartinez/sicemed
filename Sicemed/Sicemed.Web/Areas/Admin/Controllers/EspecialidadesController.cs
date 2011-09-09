using System;
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
    }
}