using System;
using System.Linq.Expressions;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class UsuariosController : CrudBaseController<Usuario>
    {
        #region Overrides of CrudBaseController<Usuario>
        protected override Expression<Func<Usuario, object>> DefaultOrderBy
        {
            get { return x => x.Membership.Email; }
        }
        #endregion
    }
}