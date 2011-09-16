using NHibernate;

namespace Sicemed.Web.Infrastructure.Controllers
{
    public class NHibernateController : BaseController
    {
        public virtual ISessionFactory SessionFactory { get; set; }
    }
}