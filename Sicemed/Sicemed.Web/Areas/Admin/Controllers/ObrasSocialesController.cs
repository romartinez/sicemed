using System;
using System.Linq.Expressions;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class ObrasSocialesController : CrudBaseController<ObraSocial>
    {
        #region Overrides of CrudBaseController<ObraSocial>

        protected override Expression<Func<ObraSocial, object>> DefaultOrderBy
        {
            get { return x => x.RazonSocial; }
        }

        #endregion
    }
}