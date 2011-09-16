using System.Web.Mvc;
using NHibernate.Util;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Controllers
{
    public class ContentController : NHibernateController
    {
        public virtual ActionResult Index(long id = 0)
        {
            var session = SessionFactory.GetCurrentSession();

            var pagina = id == 0
                             ? session.QueryOver<Pagina>().Where(p => p.Padre == null).Take(1).Future().First()
                             : session.Get<Pagina>(id);

            return pagina == null ? View("NotFound") : View(pagina);
        }
    }
}