using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Models;

namespace Sicemed.Web.Controllers
{
    public class ContentController : NHibernateController
    {
        public virtual ActionResult Index()
        {
            var session = SessionFactory.GetCurrentSession();            
            var pagina = session.QueryOver<Pagina>().Where(p => p.Padre == null).Take(1).Future().First();
            return pagina == null ? View("NotFound") : View("Content", pagina);
        }
        
        public virtual ActionResult Content(long id)
        {
            var session = SessionFactory.GetCurrentSession();
            var pagina = session.Get<Pagina>(id);
            return pagina == null ? View("NotFound") : View(pagina);
        }
    }
}