using System;
using System.Linq.Expressions;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class ConsultoriosController : CrudBaseController<Consultorio>
    {
        #region Overrides of CrudBaseController<Consultorio>

        protected override Expression<Func<Consultorio, object>> DefaultOrderBy
        {
            get { return (x) => x.Nombre; }
        }

        #endregion
    }
}