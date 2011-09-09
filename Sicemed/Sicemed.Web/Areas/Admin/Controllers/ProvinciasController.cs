using System;
using System.Linq.Expressions;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class ProvinciasController : CrudBaseController<Provincia>
    {
        #region Overrides of CrudBaseController<Provincia>

        protected override Expression<Func<Provincia, object>> DefaultOrderBy
        {
            get { return (x) => x.Nombre; }
        }

        #endregion
    }
}